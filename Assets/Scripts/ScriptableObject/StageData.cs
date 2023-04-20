using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/StageData", order = 5)]
public class StageData : ScriptableObject
{
    public int maxNpcCount;

    int npcLevel;
    public int NpcLevel
    {
        get { return npcLevel; }
        set 
        {
            if (npcLevel <= maxNpcCount) { npcLevel = value; }
            else { npcLevel = maxNpcCount; }
        }
    }

    int level;
    public int Level
    {
        get { return level; }
        set 
        { 
            level = value;
            ES3.Save(name + "level", value);
        }
    }
    public int maxLevel;

    public int[] price;
    public float[] adWeight;
    public int[] molePrice;

    public string senceName;

    public void SetStage(List<GameObject> stageList)
    {
        for (int i = 0; i < stageList.Count; i++)
        {
            if (level.Equals(i))
            {
                stageList[i].SetActive(true);
            }
            else
                break;
        }
    }

    public int GetUpgradePrice() { return price[Level]; }
}
