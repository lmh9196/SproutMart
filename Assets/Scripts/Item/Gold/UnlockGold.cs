using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockGold : GoldItem
{
    Vector3 shootPos;

    public override void GoldInit(GameManager.GoldType _goldType, Vector3 _targetPos,int _price)
    {
        base.GoldInit(_goldType, _targetPos, _price);

        spriteRenderer.color = new Color32(255, 255, 255, 255);
        GameManager.instance.ClickVib(20);

        StartCoroutine(UseGold());
    }

    IEnumerator UseGold()
    {
        float rand = Random.Range(-0.3f, 0.3f);
        shootPos = new Vector3(rand, 1, 0);
        rigid.AddForce(shootPos * 70f, ForceMode2D.Force);

        yield return new WaitForSeconds(0.51f);
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.01f);
        spriteRenderer.color -= new Color32(0, 0, 0, 15);

        StartCoroutine(FadeOut());
    }
}
