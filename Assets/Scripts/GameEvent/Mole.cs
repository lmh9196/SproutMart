using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    bool isCatchPlaying;

    [SerializeField] GameObject smokeEffect;
    public StageData stageData;

    Animator anim;

    [SerializeField] float maxActiveDuraiton;
    float activeDuration;

    CapsuleCollider2D capsuleCollider;
    MoleEvent moleEvent;
    GoldPool goldPool;

    private void Awake()
    {
        goldPool = GameObject.FindObjectOfType<GoldPool>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        moleEvent = GetComponentInParent<MoleEvent>();
    }

    private void OnEnable()
    {
        isCatchPlaying = false;
        capsuleCollider.enabled = true;
        activeDuration = 0;
        anim.SetBool("isActive", true);
    }

    private void Update()
    {
        if(!isCatchPlaying)
        {
            activeDuration += Time.deltaTime;

            if (activeDuration >= maxActiveDuraiton) { anim.SetBool("isActive", false); }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player)) { CatchAct(player); }
    }

    void CatchAct(Player _player)
    {
        capsuleCollider.enabled = false;
        isCatchPlaying = true;
        _player.attack.StartAttack(transform.position, Reward);
    }
    public void ResetMole()
    {
        gameObject.SetActive(false);
    }


    public void Reward()
    {

        goldPool.ActiveCild(goldPool.SelectGoldParent(goldPool.parentPouch),
            transform.position, GameManager.GoldType.GOLD, moleEvent.SetRewardPrice());

        Instantiate(smokeEffect, transform.position, Quaternion.identity);
      
        SoundManager.instance.PlaySfx("Smoke");

        ResetMole();
    }
}
