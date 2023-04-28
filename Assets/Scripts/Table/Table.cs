using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Table_Data
{
    public string ID;
    public string saveID;

    [Space(10f)]
    public GameManager.GoldType goldType;

    bool isUnlock;
    public bool IsUnlock
    {
        get { return isUnlock; }
        set 
        {
            isUnlock = value;
            ES3.Save(saveID + "Unlock", isUnlock);
        }
    }


    public int unlockNeedCount;
    public int unlockDefaultCount;
    public int nextStandardUnlockCount;
    public int exStandardUnlockCount;
    public int gemCount;
    public int preTableKindCount;

    public List<TableArea> tableAreaList;

    public Dictionary<string, int> areaCountDic;
}

public class Table : MonoBehaviour
{
    public Table_Data data;

    [HideInInspector] public List<Customer> customerList = new();
    [HideInInspector] public List<Transform> saleWayList = new();

    public SalesWork salesWork = new();
    public CraftWork craftWork = new();
    public CreateWork createWork = new();
    public CountWork countWork = new();
    public ExitWork exitWork = new();

    public CharData[] staffDatas;

    public List<List<Table>> preTableList = new();

    public Table[] parentTable;
    public Image lockCoinImage;
    public Transform decoParent;

    [HideInInspector] public UnlockArea lockArea;

    public virtual void Awake() { TableInit(); }
    public virtual void Start() { TableStart(); }
 
    public virtual void LateUpdate()
    {
    }

    public Action UpdateAct;
    public void TableInit()
    {
        data.tableAreaList = new();
        data.areaCountDic = new();

        data.saveID = gameObject.name;
    }

    public void UnlockInit()
    {
        lockArea = transform.GetComponentInChildren<UnlockArea>();
        lockArea.parentTable = this;
    }
    public void TableStart()
    {
        if (decoParent != null)
        {
            for (int i = 0; i < decoParent.childCount; i++) { decoParent.GetChild(i).gameObject.layer = 8; }
        }
        else { Debug.Log(gameObject.name + " Deco is Null"); }
    }
    
    public void TableReset() { customerList.Clear(); }
 
    public void ReigstStageManager()
    {
        int checkCount = 0;

        if (!StageManager.instance.salesUnLockTableList.Contains(this))
        {
            for (int i = 0; i < preTableList.Count; i++)
            {
                for (int j = 0; j < preTableList[i].Count; j++)
                {
                    if (preTableList[i][j].data.IsUnlock) { checkCount++; break; }
                }

                if (checkCount == preTableList.Count)
                {
                    StageManager.instance.salesUnLockTableList.Add(this);
                }
            }
        }
        else { return; }
    }
    
    public void RegistNeedTable(Table inputTable)
    {
        for (int i = 0; i < parentTable.Length; i++)
        {
            if (parentTable[i].preTableList.Count == 0) 
            {
                parentTable[i].preTableList.Add(new());
                parentTable[i].preTableList[0].Add(inputTable);
            }
            else
            {
                for (int j = 0; j < parentTable[i].preTableList.Count; j++)
                {
                    if (parentTable[i].preTableList[j][0].data.ID == inputTable.data.ID) { parentTable[i].preTableList[j].Add(inputTable); break; }

                    if(j == parentTable[i].preTableList.Count -1)
                    {
                        parentTable[i].preTableList.Add(new());
                        parentTable[i].preTableList[j + 1].Add(inputTable);
                    }
                }
            }
        }
    }

    public void UpdateUnlock(GameObject tableObj, GameObject unlockArea)
    {
        tableObj.gameObject.SetActive(data.IsUnlock);
        unlockArea.gameObject.SetActive(!data.IsUnlock);
    }
 
    public void UpdateCountText(SetArea[] setAreas) { for (int i = 0; i < setAreas.Length; i++) { setAreas[i].UpdateCountText(); } }
}


[Serializable]
public class ArrangementWayPoint
{
    public Transform wayPointParent;

    public enum Dir { UP, DOWN, RIGHT, LEFT }

    [Serializable]
    public struct Line
    {
        public Dir dir;
        public int count;
    }
    public Line[] line;

    [HideInInspector] public List<Vector2> childPosList = new();

    public void InitWayList(List<Transform> wayList ,Transform wayPointParent)
    {
        for (int i = 0; i < wayPointParent.childCount; i++) { wayList.Add(wayPointParent.GetChild(i)); }
    }

    public void ActiveWayPoint()
    {
        if (wayPointParent == null) { return; }

        int x = 0;
        int y = 0;

        childPosList.Add(new Vector2(x, y));

        for (int i = 0; i < line.Length; i++)
        {
            for (int j = 0; j < line[i].count; j++)
            {
                switch (line[i].dir)
                {
                    case Dir.UP:
                        y++; break;
                    case Dir.DOWN:
                        y--; break;
                    case Dir.RIGHT:
                        x++; break;
                    case Dir.LEFT:
                        x--; break;
                }

                childPosList.Add(new Vector2(x, y));
            }
        }

        for (int i = 0; i < wayPointParent.childCount; i++)
        {
            if (i >= childPosList.Count) break;

            wayPointParent.GetChild(i).localPosition = childPosList[i];
        }
    }
}


