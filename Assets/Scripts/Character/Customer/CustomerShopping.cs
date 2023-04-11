using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerShopping
{
    public void RegistShoppingList(List<Table> shoppingList ,int limitCount, Table counterTable, Table exitTable)
    {
        shoppingList.Clear();

        int randCountNum = Random.Range(1, limitCount + 1);

        if (StageManager.instance.salesUnLockTableList.Count < randCountNum)
        {
            randCountNum = StageManager.instance.salesUnLockTableList.Count;
        }

        for (int i = 0; i < randCountNum;)
        {
            int randList = Random.Range(0, StageManager.instance.salesUnLockTableList.Count);

            if (shoppingList.Contains(StageManager.instance.salesUnLockTableList[randList])) { continue; }
            else
            {
                shoppingList.Add(StageManager.instance.salesUnLockTableList[randList]);
                i++;
            }
        }

        shoppingList.Add(counterTable);
        shoppingList.Add(exitTable);
    }

    public System.Tuple<int, int> RegistShoppingCount(int handsLimitCount)
    {
        return new System.Tuple<int,int> (0, Random.Range(1, handsLimitCount));
    }

    public Transform UpdateTarget(List<Table> shoppingList, Customer thisCustomer)
    {
        if (shoppingList[0].customerList.Contains(thisCustomer))
        {
            for (int i = 0; i < shoppingList[0].customerList.Count; i++)
            {
                if (shoppingList[0].customerList[i].Equals(thisCustomer))
                {
                    if (shoppingList[0].saleWayList.Count < i) { return shoppingList[0].saleWayList[shoppingList[0].saleWayList.Count - 1].transform; }
                    
                    return shoppingList[0].saleWayList[i].transform;
                }
            }
        }
        else return shoppingList[0].transform;

        return thisCustomer.transform;
    }
}
