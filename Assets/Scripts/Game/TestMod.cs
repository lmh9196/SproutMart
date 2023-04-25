using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestMod : MonoBehaviour
{
    public static TestMod instance = null;

    //Test
    [Space(20f)]
    [Header("Test")]


   
    public StageData stageData;
    public NpcData npcData;
    public CharData[] staffData;

    private void Awake()
    {
        if(instance == null) instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        TestUpdate();
    }

    public void UnlockReset()
    {
        StageManager.instance.salesUnLockTableList.Clear();

        for (int i = 0; i < StageManager.instance.allTable.Count; i++)
        {
            if (StageManager.instance.allTable[i].data != null)
            {
                StageManager.instance.allTable[i].TableReset();
                StageManager.instance.allTable[i].data.IsUnlock = false; 
            }
        }

        NpcManager npcManager = GameObject.FindObjectOfType<NpcManager>();

        for (int i = 0; i < npcManager.transform.childCount; i++) npcManager.transform.GetChild(i).gameObject.SetActive(false);

        stageData.Level = 0;
        npcData.level = 0;
    }

    void Unlock(Transform parent, int stageNum)
    {
        List<Table> areaTable = parent.GetComponentsInChildren<Table>().ToList();

        stageData.Level = stageNum - 1;

        for (int i = 0; i < areaTable.Count; i++) { areaTable[i].data.IsUnlock = true; }
    }

    public void UnlockBtn(int stepNum)
    {
        if (stepNum >= 0)
        {
            UnlockReset();
            if (stepNum >= 1)
            {
                Unlock(StageManager.instance.areaParentList[0].transform, stepNum);
                if (stepNum >= 2)
                {
                    Unlock(StageManager.instance.areaParentList[1].transform, stepNum);
                    if (stepNum >= 3)
                    {
                        Unlock(StageManager.instance.areaParentList[2].transform, stepNum);
                        if (stepNum >= 4)
                        {
                            Unlock(StageManager.instance.areaParentList[3].transform, stepNum);
                            if (stepNum >= 5)
                            {
                                Unlock(StageManager.instance.areaParentList[4].transform, stepNum);
                                if (stepNum >= 6)
                                {
                                    Unlock(StageManager.instance.areaParentList[5].transform, stepNum);
                                }
                            }
                        }
                    }
                }
            }
        }

        GameManager.instance.AstarScanDelay();
    }

    public bool isFeedState;
    public Text feedText;
    public Text timeText;
    public int timeScaleCount = 1;

    public void TestUpdate()
    {
        if (isFeedState) feedText.text = "On";
        else feedText.text = "Off";

        timeText.text = Time.timeScale.ToString();
    }
    public void ALLFeedBtn()
    {
        if (isFeedState) isFeedState = false;

        else StartCoroutine(AutoFeedBack());
    }
  
    IEnumerator AutoFeedBack()
    {
        isFeedState = true;

        while (isFeedState)
        {
            for (int i = 0; i < StageManager.instance.allTable.Count; i++)
            {
                if (StageManager.instance.allTable[i].data.IsUnlock)
                {
                    int num;

                    for (int j = 0; j < StageManager.instance.allTable[i].data.tableAreaList.Count; j++)
                    {
                        if (StageManager.instance.allTable[i].data.tableAreaList[j].maxCount > 1) { num = 2; }
                        else { num = 1; }

                        for (; StageManager.instance.allTable[i].data.tableAreaList[j].itemBox.transform.childCount < StageManager.instance.allTable[i].data.tableAreaList[j].maxCount - num;)
                        {
                            if (StageManager.instance.allTable[i].data.tableAreaList[j].itemBox != null)
                            {
                                Crops crop = Instantiate(StageManager.instance.allTable[i].data.tableAreaList[j].cropsPre, StageManager.instance.allTable[i].data.tableAreaList[j].itemBox.transform).GetComponent<Crops>();
                                crop.CropsTableInit(StageManager.instance.allTable[i].data.tableAreaList[j].itemBox, StageManager.instance.allTable[i].data.tableAreaList[j].cropsData);
                            }
                            else
                                break;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(4f);
        }
    }

      
    public void SetStageLevel(int count) 
    {
        NpcManager npcManager = GameObject.FindObjectOfType<NpcManager>();

        for (int i = 0; i < npcManager.transform.childCount; i++)
        {
            npcManager.transform.GetChild(i).GetComponent<NormalCustomer>().DisableCustomer();
            npcManager.transform.GetChild(i).gameObject.SetActive(false);
        }
        stageData.Level = count; GameManager.instance.AstarScanDelay(); 
    }
    public void GetGold() { GameManager.instance.CalGold(GameManager.GoldType.GOLD, 10000); }
    public void GetBoxGold() { GameManager.instance.CalGold(GameManager.GoldType.BOXGOLD, 5000); }
    public void GetGem() { GameManager.instance.CalGold(GameManager.GoldType.GEM, 100); }
    public void TimeScaleBtn()
    {
        if (timeScaleCount < 5) { timeScaleCount++; }
        else { timeScaleCount = 0; }

        Time.timeScale = timeScaleCount;
    }
    public void SpawnDrunken() 
    {
        DrunkenEvent drunkenEvent = GameObject.FindObjectOfType<DrunkenEvent>();
        drunkenEvent.Spawn();
    }
    public void SpawnThief() 
    {
        ThiefEvent thiefEvent = GameObject.FindObjectOfType<ThiefEvent>();
        thiefEvent.Spawn();
    }
    public void StaffOnOff(bool isOn)
    {
        for (int i = 0; i < staffData.Length; i++)
        {
            for (int j = 0; j < staffData[i].stat.Length; j++)
            {
                staffData[i].stat[j].Level = isOn ? staffData[i].stat[j].maxLevel : 0; 
            }
        }
    }
}
