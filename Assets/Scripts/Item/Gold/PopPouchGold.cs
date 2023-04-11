using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopPouchGold : GoldItem
{
    [SerializeField] GameObject effect;


    public StageData stageData;
    public NpcData npcData;

    public override void GoldInit(GameManager.GoldType _goldType, Vector3 _targetPos, int _price)
    {
        base.GoldInit(_goldType, _targetPos,  _price);

        StartCoroutine(Act());
    }

    IEnumerator Act()
    {
        rigid.AddForce(Vector3.up * 5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.6f);

        GameManager.instance.CalGold(goldType, price);


        Instantiate(effect, transform.position, Quaternion.identity);
        SoundManager.instance.PlaySfx("RightChoice");
        GameManager.instance.ClickVib();

        gameObject.SetActive(false);
    }
}
