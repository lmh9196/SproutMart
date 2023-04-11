using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/HatData", order = 1)]
public class HatData : ScriptableObject
{
    public GameManager.GoldType goldType;
    public bool isShowHead;
    public bool isBought;
    public bool isEquip;

    public string hatName;

    [SerializeField] int price;
    [SerializeField] int gemPrice;

    public Sprite sideSprite;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite goodsSprite;

    public void RegistHatData(Hat hat) { hat._HatData = this; }

    public int HatPrice()
    {
        switch(goldType)
        {
            case GameManager.GoldType.GOLD: return price;
            case GameManager.GoldType.GEM: return gemPrice;
            default: return 0;
        }
    }
    public void SaveData(Dictionary<string, bool> hatDataDic) { hatDataDic.Add(name, isBought); }
   
    public void LoadData(Dictionary<string, bool> hatDataDic) { hatDataDic.TryGetValue(name, out isBought); }
}
