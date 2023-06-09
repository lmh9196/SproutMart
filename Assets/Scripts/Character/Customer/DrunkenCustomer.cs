using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class DrunkenCustomer : Customer
{
    bool isRight;
    public bool IsRight
    {
        get { return isRight; }
        set 
        {
            questionMark.SetActive(!value);
            exclamationMark.SetActive(value);
         
            if (value)
            {
                if(!GameManager.instance.checkList.IsTutorial_Drunken)
                {
                    DialogueManager.instance.DisableDialogue(term.termDrunken_GuideRightWay);
                    TutorialManger.instance.DiableTargetNoticeArrow(shoppingList[0].transform);
                    GameManager.instance.checkList.IsTutorial_Drunken = true;
                }
            }
            isRight = value;
        }
    }

    bool isTouch;
    public bool IsTouch { get { return isTouch; }
        set 
        {
            if(!GameManager.instance.checkList.IsTutorial_Drunken)
            {
                if (!isTouch && value)
                {
                    DialogueManager.instance.DisableDialogue(term.termDrunken_FirstNotice);
                    DialogueManager.instance.EnableDialouge(term.termDrunken_GuideRightWay, true, false);
                    TutorialManger.instance.DiableTargetNoticeArrow(transform);
                    TutorialManger.instance.ActiveTargetNoticeArrow(shoppingList[1].transform, new Vector3(0, 0.5f, 0));
                }
                else if (isTouch && !value)
                {
                    DialogueManager.instance.DisableDialogue(term.termDrunken_GuideRightWay);
                    DialogueManager.instance.EnableDialouge(term.termDrunken_FirstNotice, true, true);
                    TutorialManger.instance.DiableTargetNoticeArrow(shoppingList[1].transform);
                    TutorialManger.instance.ActiveTargetNoticeArrow(transform, new Vector3(0, 2f, 0));
                }
            }

            ResetCollider();

            isTouch = value;
        }
    }

    bool isStop;

    [SerializeField] GameObject questionMark;
    [SerializeField] GameObject exclamationMark;
    public GameObject effect;

    Player player;
    DrunkenEvent drunkenEvent;
    DialogueTerm term = new();
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindObjectOfType<Player>();
        drunkenEvent = GetComponentInParent<DrunkenEvent>();
    }

    private void OnEnable() { ResetDrunkenCustomer(); }
    private void FixedUpdate()
    {
        if (IsTouch)
        {
            customerShopping.UpdateTarget(shoppingList, this);

            if (Vector3.Distance(transform.position, player.transform.transform.position) < 1f)
                isStop = true;
            else
                isStop = false;

            if (!isStop) transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.05f);

            animDir.SetDir((player.transform.position - transform.position).normalized, isStop, player.transform);
            animDir.SetAnim();
        }
    }

    protected override void Update()
    {
        if (!IsTouch) base.Update();

        questionMark.SetActive(!IsRight);
        exclamationMark.SetActive(IsRight);
    }
  

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsRight && !IsTouch)
        {
            if (collision.CompareTag("Player"))
            {
                aiLerp.canMove = false;
                IsTouch = true;
                effect.SetActive(true);


                TargetChagne(shoppingList[0].customerList);


                SoundManager.instance.PlaySfx("Pop");
                GameManager.instance.ClickVib();

                drunkenEvent.drunkenEventBtn.IsTimer = true;
            }
        }

        if (IsTouch) { RightTouch(collision); }
    }

    void ResetDrunkenCustomer()
    {
        IsRight = false;
        IsTouch = false;
        appearance.Init(hopeImage);
        appearance.type = appearance.RegistAppearance(CustomerAppearance.Type.DRUNKEN, animDir.anim, npcData);

        customerShopping.RegistShoppingList(shoppingList, shoppingKindLimitCount, counterTable, exitTable);
        ResetHandsCount();

        if (shoppingList[0] == exitTable) { appearance.ChangeThoughtBalloon(shoppingList[0].exitWork.SelectSign()); }
        else if (shoppingList[0] == counterTable) { appearance.ChangeThoughtBalloon(shoppingList[0].countWork.hopeSprite); }
        else { appearance.ChangeThoughtBalloon(shoppingList[0].salesWork.hopeSprite); }

        StartCoroutine(SetWrongTarget());

        customerShopping.UpdateTarget(shoppingList, this);
        SetBox();
        aiLerp.SearchPath();
    }

    public IEnumerator SetWrongTarget()
    {
        while (true)
        {
            int rand = Random.Range(0, StageManager.instance.salesUnLockTableList.Count);

            if (!shoppingList.Contains(StageManager.instance.salesUnLockTableList[rand]))
            {
                shoppingList.Insert(0, StageManager.instance.salesUnLockTableList[rand]);
                break;
            }

            yield return null;
        }
    }
    public void ReturnWorngTarget()
    {
        IsTouch = false;

        effect.SetActive(true);

        StartCoroutine(ReturnDelay());
        StartCoroutine(SetWrongTarget());

        aiLerp.SearchPath();
        aiLerp.canMove = true;

        GameManager.instance.ClickVib();
    }
    public void RightTouch(Collider2D _collision)
    {
        if(_collision.TryGetComponent(out DetectArea detect))
        {
            if (IsTouch && detect.parentTable == shoppingList[0])
            {
                appearance.type = CustomerAppearance.Type.CUSTOMER;

                effect.SetActive(true);

                IsRight = true;
                IsTouch = false;
                drunkenEvent.drunkenEventBtn.IsTimer = false;

                aiLerp.SearchPath();
                aiLerp.canMove = true;

                SoundManager.instance.PlaySfx("RightChoice");
                GameManager.instance.ClickVib();
            }
        }
    }

    public IEnumerator ReturnDelay()
    {
        CapsuleCollider2D coll = GetComponent<CapsuleCollider2D>();
        coll.enabled = false;
        yield return new WaitForSeconds(2f);
        coll.enabled = true;
    }
}
