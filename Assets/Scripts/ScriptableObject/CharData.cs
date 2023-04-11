using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[SerializeField]
[CreateAssetMenu(menuName = "ScriptableObjects/CharData", order = 4)]
public class CharData : ScriptableObject
{
    public string ID;
    [Space(10f)]
    public int buyPrice;

    bool isAchiveTrigger;
    public bool IsAchiveTrigger
    {
        get { return isAchiveTrigger; }
        set 
        {
            isAchiveTrigger = value;
            MenuLockCoverAct?.Invoke();
        }
    }
    public Action MenuLockCoverAct;

    [Space(10f)]
    public int defalutMaxHandsCount;
    public float defalutCoolTime;
    public float defalutMoveSpeed;

    public Sprite[] timerImageArray;



    [Space(10f)]
    [HideInInspector] public int maxHandsCount;
    [HideInInspector] public float coolTime;
    [HideInInspector] public float buffMoveSpeed;
    [HideInInspector] public float buffCoolTime;

    public enum MenuType { COUNT ,STACK, COOLTIME }

    [Serializable]
    public class Stat
    {
        public MenuType menuType;

        [HideInInspector] public string saveID;

        int level;
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                ES3.Save(saveID + menuType.ToString() + "Level", Level);
            }
        }
        public int maxLevel;
        public int[] price;

    }
    public Stat[] stat;
  
    public void Init()
    {
        if (SetMenuType(MenuType.COUNT) != null) { SetMenuType(MenuType.COUNT).price[0] = buyPrice; }
     
        buffMoveSpeed = 0;

        for (int i = 0; i < stat.Length; i++)
        {
            stat[i].saveID = ID;
        }
    }

    public void SetUpgradePrice(MenuType menuType, int maxUpgradeStep)
    {
        Stat tempStat = null;

        for (int i = 0; i < stat.Length; i++)
        {
            if (stat[i].menuType == menuType) { tempStat = stat[i]; break; }
        }

        if (tempStat != null)
        {
            tempStat.price = new int[maxUpgradeStep];

            for (int i = 0; i < tempStat.price.Length; i++)
            {
                tempStat.price[i] = (int)((i + 1) * 0.5f * buyPrice);
            }
        }
        else { Debug.Log("Error : " + name + " is Null"); }
    }
    public void SetHandsCount() { maxHandsCount = defalutMaxHandsCount + SetMenuType(MenuType.STACK).Level; }
    public void SetCoolTime() { coolTime = defalutCoolTime - SetMenuType(MenuType.COOLTIME).Level - buffCoolTime; } 
    public float SetMoveSpeed() { return defalutMoveSpeed + buffMoveSpeed; }
    public int SetPrice(Stat stat) { return stat.price[stat.Level]; }

    public Sprite SetTimerImage(int coolTimeLevel) { return timerImageArray[coolTimeLevel]; }

    public Stat SetMenuType(MenuType menuType)
    { 
        for (int i = 0; i < stat.Length; i++)
        {
            if (stat[i].menuType == menuType)
                return stat[i];
        }
           
        return null;
    }
}
