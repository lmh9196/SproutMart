using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GoldPool : MonoBehaviour
{
    public static GoldPool instance = null;

    public enum GoldSpriteType { GOLD, GEM };

    public int kLimitCount;

    public Transform[] parentGoldAd;
    public Transform[] parentGoldCal;
    public Transform[] parentGoldUnlock;
    public Transform[] parentPouch;
    public Transform[] parentBigPouch;

    [SerializeField] Sprite[] spritePouch;
    [SerializeField] Sprite[] spriteBigPouch;
    [SerializeField] Sprite[] spriteCoin;

    GoldItem goldItem;

    public void Awake() 
    { 
        if (instance == null) { instance = this; }

        DontDestroyOnLoad(gameObject);
    }

    int CreateChild(Transform parnets)
    {
        GameObject coin = Instantiate(parnets.GetChild(0).gameObject, parnets);
        coin.SetActive(false);
        return -1; 
    }

    public void ActiveCild(Transform selectPoolParnet ,Vector3 targetPos, GameManager.GoldType goldType,int _inputGold)
    {
        for (int i = 0; i < selectPoolParnet.childCount; i++)
        {
            if (!selectPoolParnet.GetChild(i).gameObject.activeSelf)
            {
                goldItem = selectPoolParnet.GetChild(i).GetComponent<GoldItem>();
                goldItem.gameObject.SetActive(true);
                goldItem.GoldInit(goldType, targetPos, _inputGold);
                break;
            }

            if (i == selectPoolParnet.childCount - 1) { i = i + CreateChild(selectPoolParnet); }
        }
    }

    public Transform SelectGoldParent(Transform[] parentArry, bool isKGold = false)
    {
        if (isKGold) { return parentArry[1]; }
        else { return parentArry[0]; }
    }
}


