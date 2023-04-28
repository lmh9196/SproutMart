using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using DG.Tweening.Core.Easing;

[Serializable]
public class StageSaveData
{
    public string currentSceneName;
    public StageData stageData;
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    public StageSaveData saveData;
    public SaveManager saveManager;
    public StatueManager statueManager;

    public CharData[] staffDatas;
    public CraftData[] craftDatas;

    public Spawner spawner;

    public Table counterTable;
    public Table exitTable;

    public List<GameObject> stageList = new List<GameObject>();
    public List<TableParent> areaParentList = new List<TableParent>();
    [HideInInspector] public List<Table> allTable = new List<Table>();
    [HideInInspector] public List<Table> salesUnLockTableList = new List<Table>();

    [System.Serializable]
    public struct StageAutoUnlockController { public Table[] autoUnlockTable; }
    [HideInInspector] public StageAutoUnlockController[] autoUnlockStruct;
    public Table[] autoUnlockTable;
    [Space(10f)]
    public GameObject effect;

    //Tutorial
    public VideoClip burrowGuideVideo;

    public Transform startingPoint;
    public Transform area1TrasCan;

    public GameObject staffUpgradeMenu;
    public GameObject craftUpgradeMenu;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        RegistAllTable();
        //RegistNeedTable();

        Instantiate(staffUpgradeMenu, MenuManager.instance.upgrade.staffPage);
        Instantiate(craftUpgradeMenu, MenuManager.instance.upgrade.machinePage);

        for (int i = 0; i < staffDatas.Length; i++) { staffDatas[i].Init(); }
    }

    private void Start()
    {
        for (int i = 0; i < staffDatas.Length; i++)
        {
            for (int j = 0; j < staffDatas[i].stat.Length; j++)
            {
                string key = staffDatas[i].ID + staffDatas[i].stat[j].menuType.ToString() + "Level";

                if (ES3.KeyExists(key)) { staffDatas[i].stat[j].Level = ES3.Load<int>(key); }
                else { staffDatas[i].stat[j].Level = 0; }
            }
        }

        for (int i = 0; i < craftDatas.Length; i++)
        {
            for (int j = 0; j < craftDatas[i].stat.Length; j++)
            {
                string key = craftDatas[i].name + craftDatas[i].stat[j].menuType.ToString() + "Level";

                if (ES3.KeyExists(key)) {craftDatas[i].stat[j].Level = ES3.Load<int>(key); }
                else { craftDatas[i].stat[j].Level = 0; }
            }
        }
    }


    public void InitStage()
    {
        GameManager.instance.data.saveSceneName = saveData.currentSceneName;
        saveManager.statueManager = statueManager;
    }
    public void StageAffterLoad()
    {
        if (GameManager.instance.checkList.IsTutorialEnd) { Player.instance.transform.position = startingPoint.position; }

        MainCamera.instance.ChangeCamTarget(false);

        for (int j = 0; j < autoUnlockTable.Length; j++) { autoUnlockTable[j].data.IsUnlock = true; }

        GameManager.instance.canvasList.Init();

        GameManager.instance.AstarScanDelay();
    }


    private void Update()
    {
        if (area1TrasCan != null) { TutorialManger.instance.TutorialTrashCan(Player.instance, area1TrasCan); }

        for (int i = 0; i < stageList.Count; i++) { stageList[i].SetActive(saveData.stageData.Level == i); }

        for (int i = 0; i < areaParentList.Count; i++)
        {
            if(saveData.stageData.Level >= i)
            {
                if (!areaParentList[i].gameObject.activeSelf) { areaParentList[i].gameObject.SetActive(true); }
            }
            else
            {
                if (areaParentList[i].gameObject.activeSelf) { areaParentList[i].gameObject.SetActive(false); }
            }
        }

        for (int i = 0; i < allTable.Count; i++)
        {
            if (allTable[i] is CraftGetTable craftGet) { UpdateCheckCraftLock(craftGet, craftGet.craftData); }
            else if (allTable[i] is CraftSalesTable craftSales) { UpdateCheckCraftLock(craftSales, craftSales.craftData); }
        }

        MenuManager.instance.expand.UpdateNpcLvText(saveData.stageData);
    }

    public void RegistAllTable()
    {
        for (int i = 0; i < areaParentList.Count; i++)
        {
            Table[] tableArry = areaParentList[i].GetComponentsInChildren<Table>(true);

            for (int j = 0; j < tableArry.Length; j++)
            {
                areaParentList[i].childTable.Add(tableArry[j]);
                allTable.Add(tableArry[j]);
                
                tableArry[j].TableInit();
                RegistNeedTable();
            }

            areaParentList[i].AssignDefalutUnlockCount();
        }
    }

    public void RegistNeedTable()
    {
        for (int i = 0; i < allTable.Count; i++)
        {
            if (allTable[i] is CraftGetTable craftGet) { craftGet.RegistNeedTable(craftGet); }
            else if (allTable[i] is CreateTable create) { create.RegistNeedTable(create); }
        }
    }
  
    public IEnumerator MartEvent()
    {
        GameObject camPoint = areaParentList[saveData.stageData.Level + 1].transform.Find("CamPoint").gameObject;
        Instantiate(effect, camPoint.transform);
        MainCamera.instance.CamEvent(camPoint, 6f);
        yield return new WaitForSeconds(3f);

        saveData.stageData.Level++;
        yield return null;

        SoundManager.instance.PlaySfx("Smoke");
        GameManager.instance.ClickVib();

        yield return new WaitForSeconds(3.5f);

        if (saveData.stageData.Level == 2) VideoManager.instance.VideoPlay(burrowGuideVideo, 0);
        GameManager.instance.AstarScanDelay();

        yield return new WaitForSeconds(0.5f);

        Destroy(camPoint.transform.GetChild(0).gameObject);
    }

    public void UpdateCheckCraftLock(Table table, CraftData craftData)
    {
        craftData.IsAchiveTrigger = table.transform.parent.gameObject.activeSelf && table.data.IsUnlock ? true : false;
    }
}

[Serializable]
public class Spawner
{
    public Transform[] spawnerArray;
    public Vector3 SelectSpawner()
    {
        int count = 0;

        for (int i = 0; i < spawnerArray.Length; i++)
        {
            if (spawnerArray[i].parent.gameObject.activeSelf) count = i + 1;
            else break;
        }

        int selectNum = UnityEngine.Random.Range(0, count);

        return spawnerArray[selectNum].position;
    }
}