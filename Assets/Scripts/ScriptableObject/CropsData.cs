using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/CropsData", order = 2)]
public class CropsData : ScriptableObject
{
    public int price;

    public Sprite frontSprite;
    public Sprite sideSprite;
}
