using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetArea : TableArea
{
    [Space(20f)]
    [SerializeField] Text countText;


    public override void Start()
    {
        base.Start();
    }
    public void UpdateCountText() 
    { 
        if(countText != null)
        {
            countText.text = itemBox.childCount + "/" + maxCount.ToString();
        }
    }
}
