using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADGiftItem : GoldItem
{
    [SerializeField] Sprite openSprite;

    [SerializeField] GameObject disableEffect;
    [SerializeField] ParticleSystem dustEffect;

    public override void GoldInit(GameManager.GoldType _goldType, Vector3 _targetPos, int _price)
    {
        base.GoldInit(_goldType, _targetPos, _price);

        rigid.constraints = RigidbodyConstraints2D.None;
        GetComponent<CircleCollider2D>().enabled = false;

        StartCoroutine(Act());
    }
    IEnumerator Act()
    {
        float playerYpos = Player.instance.transform.position.y;

        while (true)
        {
            if((transform.position.y - playerYpos) < 0.1f)
            {
                rigid.constraints = RigidbodyConstraints2D.FreezeAll;
                SoundManager.instance.PlaySfx("Land");
                dustEffect.Play();

                yield return null;
                GetComponent<CircleCollider2D>().enabled = true;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player)) 
        {
            SoundManager.instance.PlaySfx("Sell");
            SoundManager.instance.PlaySfx("Pop");
            GameManager.instance.ClickVib();

            GameManager.instance.CalGold(goldType, price);
            Instantiate(disableEffect, transform.position, Quaternion.identity);


            gameObject.SetActive(false);
        }
    }
}
