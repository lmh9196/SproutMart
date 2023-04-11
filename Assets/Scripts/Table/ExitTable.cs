using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTable : Table
{
    public Sprite[] hopeSprite;

    public override void Awake()
    {
        base.Awake();

        exitWork.Init(hopeSprite);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Npc")) { collision.gameObject.SetActive(false); }
    }
}
