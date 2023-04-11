using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEditor.Rendering;

[Serializable]
public class SaveManager : MonoBehaviour
{
    public static SaveManager instance = null;

    public StageManager stageManager;
    [HideInInspector] public StatueManager statueManager;


    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            GameLoad(); return;
        }
        else 
        {
            if (GameManager.instance.data.saveSceneName != stageManager.saveData.currentSceneName)
            {
                LoadingManager.instance.StartLoading(GameManager.instance.data.saveSceneName);
            }
            else
            {
                StageLoad();
            }
        }
    }

    private async void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            await Task.Run(GameSave);
        }
    }
    private async void OnApplicationQuit()
    {
        await Task.Run(GameSave);
    }

    public void GameLoad()
    {
        if (ES3.KeyExists("SaveSceneName")) { GameManager.instance.data.saveSceneName = ES3.Load<string>("SaveSceneName"); }
        else { GameManager.instance.data.saveSceneName = "Game_Stage1"; }

        if (ES3.KeyExists("Tutorial")) { GameManager.instance.checkList.IsTutorialEnd = ES3.Load<bool>("Tutorial"); }
        else { GameManager.instance.checkList.IsTutorialEnd = false; }

        if (ES3.KeyExists("Tutorial_Drunken")) { GameManager.instance.checkList.IsTutorial_Drunken = ES3.Load<bool>("Tutorial_Drunken"); }
        else { GameManager.instance.checkList.IsTutorial_Drunken = false; }

        if (ES3.KeyExists("Tutorial_Full")) { GameManager.instance.checkList.IsTutorial_Full = ES3.Load<bool>("Tutorial_Full"); }
        else { GameManager.instance.checkList.IsTutorial_Full = false; }


        if (ES3.KeyExists("GameGold")) { GameManager.instance.data.gold = ES3.Load<int>("GameGold"); }
        else { GameManager.instance.data.gold = 0; }

        if (ES3.KeyExists("GameBoxGold")) { GameManager.instance.data.boxGold = ES3.Load<int>("GameBoxGold"); }
        else { GameManager.instance.data.boxGold = 0; }

        if (ES3.KeyExists("Gem")) { GameManager.instance.data.gem = ES3.Load<int>("Gem"); }
        else { GameManager.instance.data.gem = 0; }


        if (ES3.KeyExists("MainCamera")) { MainCamera.instance.data.camSize = ES3.Load<float>("MainCamera"); }
        else { MainCamera.instance.data.camSize = 10; }

        MenuManager.instance.setting.LoadData();

        if (ES3.KeyExists("NoticeTargetList"))
        { MenuManager.instance.noticeArrow.noticeBtnNameList = ES3.Load("NoticeTargetList", MenuManager.instance.noticeArrow.noticeBtnNameList); }
        else { MenuManager.instance.noticeArrow.noticeBtnNameList.Clear(); }


        for (int i = 0; i < Player.instance.charData.stat.Length; i++)
        {
            string playerLvKey = (Player.instance.charData.ID + Player.instance.charData.stat[i].menuType.ToString() + "Level");

            if (ES3.KeyExists(playerLvKey)) { Player.instance.charData.stat[i].Level = ES3.Load<int>(playerLvKey); }
        }
        


        if (ES3.KeyExists("HatName"))
        {
            string name = ES3.Load<string>("HatName");

            for (int i = 0; i < Player.instance.hat.hatDatas.Length; i++)
            {
                if (Player.instance.hat.hatDatas[i].name == name) { Player.instance.hat._HatData = Player.instance.hat.hatDatas[i]; }
            }
        }
        else { Player.instance.hat._HatData = Player.instance.hat.unwearHat; }

        LoadingManager.instance.StartLoading(GameManager.instance.data.saveSceneName);

        GameManager.instance.checkList.IsTutorial_Full = false;
    }
 
    public void StageLoad()
    {
        stageManager.InitStage();

        if (ES3.KeyExists(stageManager.saveData.stageData.name + "level")) { stageManager.saveData.stageData.Level = ES3.Load<int>(stageManager.saveData.stageData.name + "level"); }
        else { stageManager.saveData.stageData.Level = 0; }

        for (int i = 0; i < stageManager.allTable.Count; i++)
        {
            Table_Data tableData = stageManager.allTable[i].data;

            if (ES3.KeyExists(tableData.saveID + "Unlock")) { tableData.IsUnlock = ES3.Load<bool>(tableData.saveID + "Unlock"); }
            else { tableData.IsUnlock = false; }

            if (ES3.KeyExists(tableData.saveID + "NeedCount")) { tableData.unlockNeedCount = ES3.Load<int>(tableData.saveID + "NeedCount"); }
            else { tableData.unlockNeedCount = tableData.unlockDefaultCount; }


            for (int j = 0; j < tableData.tableAreaList.Count; j++)
            {
                string key = tableData.saveID + tableData.tableAreaList[j].iD + "Count";

                if (ES3.KeyExists(key))
                {
                    tableData.tableAreaList[j].currentCount = ES3.Load<int>(key);
                    tableData.tableAreaList[j].LoadCrops();
                }
                else { tableData.tableAreaList[j].currentCount = 0; }
            }
        }

        stageManager.StageAffterLoad();
    }

    public void GameSave()
    {
        if (!GameManager.instance.checkList.IsTutorialEnd) { ES3.DeleteFile("SaveFile.es3"); return; }

        ES3.Save("SaveSceneName", GameManager.instance.data.saveSceneName);

        ES3.Save("GameGold", GameManager.instance.data.gold);
        ES3.Save("GameBoxGold", GameManager.instance.data.boxGold);
        ES3.Save("Gem", GameManager.instance.data.gem);

        MenuManager.instance.setting.SaveData();

        ES3.Save("NoticeTargetList", MenuManager.instance.noticeArrow.noticeBtnNameList);

        ES3.Save("MainCamera", MainCamera.instance.data.camSize);

        StageSave();
    }
    public void StageSave()
    {
        for (int i = 0; i < stageManager.allTable.Count; i++)
        {
            Table_Data tableData = stageManager.allTable[i].data;

            //ES3.Save(tableData.saveID + "Unlock", tableData.IsUnlock);
            ES3.Save(tableData.saveID + "NeedCount", tableData.unlockNeedCount);

            for (int j = 0; j < tableData.tableAreaList.Count; j++)
            {
                ES3.Save(tableData.saveID + tableData.tableAreaList[j].iD + "Count", tableData.tableAreaList[j].currentCount);
            }
        }
    }
}

