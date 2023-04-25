using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetArea : TableArea, IMoveCrop
{
    [Space(20f)]
    [SerializeField] Text countText;


    public override void Start()
    {
        base.Start();
    }
    public void UpdateCountText() 
    {
        if (countText != null) { countText.text = itemBox.childCount + "/" + maxCount.ToString(); }
    }

    public void Move(Transform charItemBox, int charMaxCount, Action FeedBackAct = null)
    {
        if (itemBox.transform.childCount < maxCount)
        {
            for (int i = 0; i < charItemBox.childCount; i++)
            {
                Crops crops = charItemBox.GetChild(charItemBox.childCount - 1 - i).GetComponent<Crops>();

                if (crops.cropsData.Equals(cropsData))
                {
                    crops.CropsTableInit(itemBox, cropsData);
                    FeedBackAct?.Invoke();
                    break;
                }
            }
        }
    }
}
