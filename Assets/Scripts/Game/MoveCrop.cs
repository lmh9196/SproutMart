using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCrop
{
    public bool isMoveRunning;
    public bool isTouch;
    public IEnumerator MoveCrops(string type , TableArea _area,  Transform charItemBox, int charCount = 0, Action FeedBackAct = null)
    {
        isMoveRunning = true;

        while (isTouch)
        {
            switch (type)
            {
                case "Get": GetCrops(_area, charItemBox, charCount, FeedBackAct); break;
                case "Set": SetCrops(_area, charItemBox, FeedBackAct); break;
                default: Debug.Log("Error: Wrong Area"); break;
            }
            yield return new WaitForSeconds(0.2f);
        }

        isMoveRunning = false;
    }

    public void SalesCrop(TableArea _area, Customer customer)
    {
        if (_area.itemBox.transform.childCount > 0)
        {
            _area.itemBox.transform.GetChild(_area.itemBox.transform.childCount - 1).TryGetComponent(out Crops crops);
            crops.ChangeParent(crops.cropsData, customer.itemBox, Crops.ParentsType.CHAR);
            customer.currentHandsCount++;
        }
    }

    void GetCrops(TableArea _area,Transform charItemBox, int charMaxCount, Action FeedBackAct)
    {
        if (_area.itemBox.transform.childCount > 0 && charItemBox.childCount < charMaxCount)
        {
            _area.itemBox.transform.GetChild(_area.itemBox.transform.childCount - 1).TryGetComponent(out Crops crops);
            crops.ChangeParent(crops.cropsData, charItemBox, Crops.ParentsType.CHAR);

            FeedBackAct?.Invoke();
        }
    }

    void SetCrops(TableArea _area, Transform charItemBox, Action FeedBackAct)
    {
        if (_area.itemBox.transform.childCount < _area.maxCount)
        {
            for (int i = 0; i < charItemBox.childCount; i++)
            {
                charItemBox.GetChild(charItemBox.childCount - 1 - i).TryGetComponent(out Crops crops);

                if (crops.cropsData.Equals(_area.cropsData))
                {
                    crops.ChangeParent(crops.cropsData, _area.itemBox.transform, Crops.ParentsType.TABLE);
                    FeedBackAct?.Invoke();
                    break;
                }
            }
        }
    }
}
