using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;

public class Thief : MonoBehaviour
{
    int currentGold;

    bool isSteelTrigger;
    bool isSteelDone;

    [SerializeField] GameObject steelTrigger;
    Vector3 dir;
    [SerializeField] GameObject smokeEffect;
    [SerializeField] GameObject moneyPre;

    [SerializeField] GameObject moneyImage;
    [SerializeField] GameObject emoteImage;

    public GoldBox goldBox;
    ExitTable exit;

    Animator anim;
    [HideInInspector] public AILerp aILerp;
    AIDestinationSetter aIDestinationSetter;
    Rigidbody2D rigid;

    CapsuleCollider2D capsuleCollider;
    SortingGroup sortingGroup;
    GoldPool goldPool;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        exit = GameObject.FindObjectOfType<ExitTable>();
        goldPool = GameObject.FindObjectOfType<GoldPool>();
        aILerp = GetComponent<AILerp>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();
        sortingGroup = GetComponent<SortingGroup>();
    }


    private void OnEnable() { Init(); }
    void Init()
    {
        aIDestinationSetter.target = steelTrigger.transform;
        currentGold = 0;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        capsuleCollider.enabled = true;
        aILerp.canMove = true;
        sortingGroup.sortingOrder = 0;
        isSteelDone = false;
        isSteelTrigger = false;
    }

    private void Update()
    {
        SetDir();

        if (currentGold > 0)
        {
            emoteImage.SetActive(false);
            moneyImage.SetActive(true);
        }
        else
        {
            emoteImage.SetActive(true);
            moneyImage.SetActive(false);
        }

        if (aILerp.reachedEndOfPath) { dir = Vector3.up; }

        SetAnim();
    }

    void SetDir()
    {
        if (Mathf.Abs(aILerp.Dir.x) > Mathf.Abs(aILerp.Dir.y))
        {
            if (aILerp.Dir.x > 0) { dir = Vector3.right; }
            else { dir = Vector3.left; }
        }
        else
        {
            if (aILerp.Dir.y > 0) { dir = Vector3.up; }
            else { dir = Vector3.down; }
        }
    }
    
    void SetAnim()
    {
        anim.SetFloat("fHorizontal", dir.x);
        anim.SetFloat("fVertical", dir.y);

        if (!aILerp.reachedEndOfPath) { anim.SetBool("isMove", true); }
        else { anim.SetBool("isMove", false); }
    }

    IEnumerator SteelEvent()
    {
        isSteelTrigger = true;
        yield return null;

        if (GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD) > 0)
        {
            goldBox.anim.SetTrigger("Open");
            currentGold = GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD);
            GameManager.instance.AssignGold(GameManager.GoldType.BOXGOLD, 0);


            yield return new WaitForSeconds(4.5f);
            aIDestinationSetter.target = exit.transform;

            isSteelTrigger = false;
            isSteelDone = true;
        }
        else
        {
            StartCoroutine(SteelEvent());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player)) { StartCatch(player); }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(steelTrigger)) { if (!isSteelTrigger && !isSteelDone) StartCoroutine(SteelEvent()); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(steelTrigger))
        {
            goldBox.anim.SetTrigger("Close");
        }
    }

    void StartCatch(Player _player)
    {
        capsuleCollider.enabled = false;
        aILerp.canMove = false;
        _player.attack.StartAttack(transform.position, CatchFeedBack);

        if (_player.transform.position.y < transform.position.y) { sortingGroup.sortingOrder = -1; }
        else { sortingGroup.sortingOrder = 1; }
    }

    void CatchFeedBack()
    {
        anim.SetTrigger("Hit");
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.AddForce(Vector3.up * 3f, ForceMode2D.Impulse);
    }

    public void EndAct()
    {
        SoundManager.instance.PlaySfx("Smoke");
        Instantiate(smokeEffect, rigid.transform.position, Quaternion.identity);

        int dropGold = 10;

        if (currentGold > 0) { dropGold = currentGold + 10; }

        goldPool.ActiveCild(goldPool.SelectGoldParent(goldPool.parentPouch), transform.position, GameManager.GoldType.GOLD, dropGold);

        gameObject.SetActive(false);
    }
}
