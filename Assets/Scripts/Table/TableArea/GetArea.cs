using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetArea : TableArea, IMoveCrop
{
    public override void Start()
    {
        base.Start();
    }

    public void Move(Transform charItemBox, int charMaxCount, Action FeedBackAct = null)
    {
        if (itemBox.transform.childCount > 0 && charItemBox.childCount < charMaxCount)
        {
            itemBox.transform.GetChild(itemBox.transform.childCount - 1).TryGetComponent(out Crops crops);
            crops.CropsCharInit(charItemBox, cropsData);
            FeedBackAct?.Invoke();
        }
    }
}
