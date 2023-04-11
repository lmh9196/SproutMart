using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GoldPool;

public class GoldItem : MonoBehaviour
{
    [HideInInspector] public GameManager.GoldType goldType;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public int price;

    [SerializeField] Sprite[] spriteType;

    public virtual void GoldInit(GameManager.GoldType _goldType ,Vector3 _startPos, int _price)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        goldType = _goldType;

        GoldSpriteType goldSpriteType;
        switch (goldType)
        {
            case GameManager.GoldType.GEM:
                goldSpriteType = GoldSpriteType.GEM; break;

            default:
                goldSpriteType = GoldSpriteType.GOLD; break;
        }

        spriteRenderer.sprite = spriteType[(int)goldSpriteType];

        transform.position = _startPos;
        price = _price;
    }
}
