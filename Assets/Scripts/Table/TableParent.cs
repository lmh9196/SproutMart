using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TableParent : MonoBehaviour
{
    public List<Table> childTable = new();
    Dictionary<string, List<Table>> tableDic = new();

    [SerializeField] Sprite gemCoinImage;
    [SerializeField] Sprite goldCoinImage;


    public void AssignDefalutUnlockCount()
    {
        for (int i = 0; i < childTable.Count; i++)
        {
            if (tableDic.ContainsKey(childTable[i].data.ID)) { tableDic[childTable[i].data.ID].Add(childTable[i]); }
            else { tableDic[childTable[i].data.ID] = new List<Table> { childTable[i] }; }

            List<Table> tableList = tableDic[childTable[i].data.ID];
            float constWeight = CompareTypeWeight(tableList[0]);

            for (int j = 0; j < tableList.Count; j++)
            {
                if (tableList[j].data.gemCount > 0)
                {
                    tableList[j].data.goldType = GameManager.GoldType.GEM;
                    tableList[j].lockCoinImage.sprite = gemCoinImage;
                    tableList[j].data.unlockDefaultCount = tableList[j].data.gemCount;
                }
                else
                {
                    tableList[j].data.goldType = GameManager.GoldType.GOLD;
                    tableList[j].lockCoinImage.sprite = goldCoinImage;

                    if (tableList[j].data.exStandardUnlockCount == 0)
                    {
                        tableList[j].data.unlockDefaultCount
                     = SettingChildUnlockPrice(tableList[j].data.nextStandardUnlockCount, j, tableList.Count, constWeight);
                    }
                    else { tableList[j].data.unlockDefaultCount = tableList[j].data.exStandardUnlockCount; }
                }

                tableList[j].data.unlockNeedCount = tableList[j].data.unlockDefaultCount;
            }
        }
    }

    int SettingChildUnlockPrice(int standardCount, int count, int maxCount, float constWeight)
    {
        float calStandard = standardCount * constWeight;
        float weight = (( 1f/ (float)maxCount) * calStandard);

        return ((int)((weight * count) + calStandard) - (int)((weight * count) + calStandard)%10);
    }

    float CompareTypeWeight(Table table)
    {
        if (table as CraftGetTable) { return 0.3f; }
        else if (table as CreateTable) { return 0.3f; }
      
        else { return 0.7f; }
    }
}
