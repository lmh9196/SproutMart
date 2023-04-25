using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TableArea : MonoBehaviour
{
    [HideInInspector] public Table parentTable;

    public string iD;

    public CropsData cropsData;
    public StaffController staffController;
    public GameObject cropsPre;

    public Transform itemBox;
    public int currentCount; 

    public int maxCount;
    public bool isEnter;

    [Space(20f)]
    public ArrangementTableItem tableItem;

    public void InitTableArea(Table parentTable)
    {
        this.parentTable = parentTable;
        parentTable.data.tableAreaList.Add(this);
        parentTable.data.areaCountDic.Add(gameObject.name, 0);
        iD = cropsData.name;
    }


    public void Start()
    {
        tableItem.StartPreSet();
        if (tableItem.itemPosList.Count < maxCount) { maxCount = tableItem.itemPosList.Count; }
    }
    
    public void LoadCrops()
    {
        for (int i = 0; i < currentCount; i++)
        {
            Crops crop = Instantiate(cropsPre, itemBox.transform).GetComponent<Crops>();
            crop.CropsTableInit(itemBox , cropsData);
        }
    }

    public void Update() 
    {
        tableItem.UpdateSetTableitemPos(itemBox);
    }
    public void LateUpdate()
    {
        currentCount = itemBox.childCount;

        parentTable.data.areaCountDic[gameObject.name] = currentCount;
    }

    public int SetNeedCount() { return maxCount - itemBox.childCount; }
}


[Serializable]
public class ArrangementTableItem
{
    public List<Vector2> itemPosList = new();

    public Transform itemPosParent;

    public enum PutDir { Vertical, Horizontal, Manual }
    public PutDir putDir;

    public int maxVerticalSize;
    [SerializeField] float vSpacing;
    public int maxHorizontalSize;
    [SerializeField] float hSpacing;

    Vector2[,] itemPosArray;

    public void StartPreSet()
    {
        for (int i = 0; i < itemPosParent.transform.childCount; i++)
        {
            itemPosParent.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }

        switch (putDir)
        {
            case PutDir.Manual: CreateManualMap(); return;
            default: CreateAutoMap(); return;
        }
    }
    public void UpdateSetTableitemPos(Transform itemBox)
    {
        for (int i = 0; i < itemBox.childCount; i++) 
        {
            if (itemPosList.Count > i) { itemBox.GetChild(i).localPosition = itemPosList[i]; }
        }
    }

    void CreateAutoMap()
    {
        itemPosArray = new Vector2[maxHorizontalSize, maxVerticalSize];

        int firstDir = 0;
        int secondDir = 0;
        float firstSpacing = 0;
        float secondSpacing = 0;
        Vector2 startPos = itemPosParent.GetChild(0).localPosition;

        switch (putDir)
        {
            case PutDir.Vertical:
                firstDir = maxVerticalSize;
                secondDir = maxHorizontalSize;
                firstSpacing = vSpacing;
                secondSpacing = hSpacing;
                break;
            case PutDir.Horizontal:
                firstDir = maxHorizontalSize;
                secondDir = maxVerticalSize;
                firstSpacing = hSpacing;
                secondSpacing = vSpacing;
                break;
        }

        for (int i = 0; i < secondDir; i++)
        {
            for (int j = 0; j < firstDir; j++)
            {
                if (i == 0 && j == 0)
                {
                    itemPosArray[0, 0] = startPos;
                    itemPosList.Add(itemPosArray[0, 0]);
                }
                else
                {
                    switch (putDir)
                    {
                        case PutDir.Vertical:
                            itemPosArray[i, j] = (itemPosArray[0, 0] + new Vector2(secondSpacing * i, firstSpacing * j));
                            itemPosList.Add(itemPosArray[i, j]);
                            break;
                        case PutDir.Horizontal:
                            itemPosArray[j, i] = (itemPosArray[0, 0] + new Vector2(firstSpacing * j, secondSpacing * i));
                            itemPosList.Add(itemPosArray[j, i]);
                            break;
                    }
                }
            }
        }
    }

    void CreateManualMap()
    {
        if (itemPosParent.childCount > 0)
        {
            for (int i = 0; i < itemPosParent.childCount; i++) 
            {
                itemPosList.Add(itemPosParent.GetChild(i).localPosition); 
            }
        }
    }
}
