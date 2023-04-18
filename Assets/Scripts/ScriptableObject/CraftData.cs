using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/CraftData", order = 3)]
public class CraftData : ScriptableObject
{
    bool isAchiveTrigger;
    public bool IsAchiveTrigger
    {
        get { return isAchiveTrigger; }
        set { isAchiveTrigger = value; }
    }

    [Header("Table")]
    public float defalutMakingTime;

    public float buffSpeed;
    public enum MenuType { STACK, SPEED }

    [Serializable]
    public class Stat
    {
        public MenuType menuType;
        int level;

        [HideInInspector] public string saveID;
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
        public bool isMax;
        public int[] needPrice;
    }

    public Stat[] stat;

    public void StatReset() { for (int i = 0; i < stat.Length; i++) { stat[i].Level = 0; } }
  
    public float ResultMakeSpeed() 
    {
        return SetMakeSpeed() - buffSpeed;
    }
    public float SetMakeSpeed() { return defalutMakingTime - (defalutMakingTime * (SetMenuType(MenuType.SPEED).Level * 0.16f)); }
    public int SetPrice(Stat stat) 
    {
        return stat.needPrice[stat.Level];
    }

    public void Init()
    {
        buffSpeed = 0;

        for (int i = 0; i < stat.Length; i++) { stat[i].saveID = name; }
    }

    public void InitNeedPrice(int unlockDefaultPrice)
    {
        for (int i = 0; i < stat.Length; i++)
        {
            stat[i].needPrice = new int[stat[i].maxLevel];

            for (int j = 0; j < stat[i].needPrice.Length; j++)
            {
                stat[i].needPrice[j] = (int)((j + 1) * 0.5f * unlockDefaultPrice);
            }
        }
    }

    public Stat SetMenuType(MenuType menuType) 
    { 
        for (int i = 0; i < stat.Length; i++)
        {
            if (stat[i].menuType == menuType) { return stat[i]; }
        }
        return null; 
    }
}

