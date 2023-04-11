using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat : MonoBehaviour
{
    [SerializeField] Sprite nonGrowth;
    [SerializeField] Sprite Growth;


    public SpriteRenderer targetSpriteRenderer;
    public Transform itemBox;

 

    void Update()
    {
        if (itemBox.childCount > 0)
        {
            targetSpriteRenderer.sprite = Growth;
        }
        else
        {
            targetSpriteRenderer.sprite = nonGrowth;
        }
    }
}
