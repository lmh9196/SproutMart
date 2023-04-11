using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GoldBox : MonoBehaviour
{
    bool isSpawnPlaying;

    [SerializeField] Transform goldPoint;

    [HideInInspector] public Animator anim;
    AudioSource audioSource;

    bool tempCheckBool;
    private void Awake()
    {
        audioSource =  GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(tempCheckBool)
        {
            if (!isSpawnPlaying) { anim.SetTrigger("Close"); }
        }

        tempCheckBool = isSpawnPlaying;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isSpawnPlaying) { StartCoroutine(GoldSpawn()); }
        }
    }

    IEnumerator GoldSpawn()
    {
        isSpawnPlaying = true;

        int kGold = GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD) / 1000;
        int remainGold = (GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD) % 1000) / 10;
        int smallGold = (GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD) % 1000) % 10;

        if (GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD) > 0) { audioSource.Play(); }
        GameManager.instance.ClickVib();
        anim.SetTrigger("Open");

        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < kGold; i++) 
        { 
            BoxCalGold(1000, true);
            yield return new WaitForSeconds(0.33f);
        }

        for (int i = 0; i < remainGold; i++)
        {
            BoxCalGold(10, false);
            yield return new WaitForSeconds(0.05f);
        }

        if (smallGold > 0) { BoxCalGold(GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD), false); }
        isSpawnPlaying = false;
    }

    void BoxCalGold(int inputGold, bool isK)
    {
        GoldPool.instance.ActiveCild(GoldPool.instance.SelectGoldParent(GoldPool.instance.parentGoldCal, isK), goldPoint.position, GameManager.GoldType.BOXGOLD, inputGold);
        GameManager.instance.CalGold(GameManager.GoldType.BOXGOLD, -inputGold);
        GameManager.instance.CalGold(GameManager.GoldType.GOLD, inputGold);
    }
}
