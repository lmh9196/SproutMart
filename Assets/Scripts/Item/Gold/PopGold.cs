using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopGold : GoldItem
{
    Vector3 shootPos;

    [SerializeField] GameObject disableEffect;
    public override void GoldInit(GameManager.GoldType _goldType, Vector3 _targetPos, int _price)
    {
        base.GoldInit(_goldType, _targetPos,  _price);

        StartCoroutine(Act());
    }
  
    IEnumerator Act()
    {
        float rand = Random.Range(-0.4f, 0.4f);
        shootPos = new Vector3(rand, 1, 0);
        rigid.AddForce(shootPos * 250f, ForceMode2D.Force);
        yield return new WaitForSeconds(1.2f);

        gameObject.SetActive(false);
        Instantiate(disableEffect, transform.position, Quaternion.identity);
    }
}
