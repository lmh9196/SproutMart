using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager instance;

    bool isGem;
    public bool IsGem
    {
        get { return isGem; }
        set
        {

            if (value) { unlockCheckGemBtn.SetActive(true); }
            else { unlockCheckGemBtn.SetActive(false); }

            isGem = value;
        }
    }


    bool isAct;
    public bool IsAct
    {
        get { return isAct; }
        set
        {
            if (!value) { actTable = null; }
            isAct = value;
        }
    }

    Table actTable;
    public StageData stageData;
    public GameObject unlockCheckGemBtn;

    [HideInInspector] public UnlockArea unlockArea;
    private void Awake() 
    { 
        if (instance == null) instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(IsAct && actTable != null) { Unlocking(); }
     
        unlockCheckGemBtn.SetActive(IsAct && IsGem);
    }

    public void UnlockBtnCheck() 
    {
        IsAct = true;
        IsGem = false;
    }

    public void CheckUnlockType(Table table)
    {
        if (GameManager.instance.SelectGold(table.data.goldType) > 0)
        {
            switch (table.data.goldType)
            {
                case GameManager.GoldType.GEM: IsGem = true;  break;
                default: IsAct = true; break;
            }
            actTable = table;
        }
    }

    public float touchtimer;
    public void Unlocking()
    {
        int gold = SetCalCount();

        if(gold > 0)
        {
            if (GameManager.instance.SelectGold(actTable.data.goldType) >= gold)
            {
                touchtimer += Time.deltaTime;

                if (touchtimer >= 0.1f)
                {
                    touchtimer = 0;
                    GoldPool.instance.ActiveCild
                        (GoldPool.instance.SelectGoldParent(GoldPool.instance.parentGoldUnlock), actTable.lockArea.transform.position, actTable.data.goldType, gold);

                    actTable.data.unlockNeedCount -= gold;
                    GameManager.instance.CalGold(actTable.data.goldType, -gold);
                }
            }
        }
    }

    int SetCalCount()
    {
        int calCount = actTable.data.unlockDefaultCount / 10;
        int calGold = 0;

        if (actTable.data.unlockNeedCount >= calCount) { calGold = calCount; }
        else { calGold = actTable.data.unlockNeedCount; }
       
        if (GameManager.instance.SelectGold(actTable.data.goldType) < calGold) {return GameManager.instance.SelectGold(actTable.data.goldType); }
        else { return calGold; }
    }
}
