using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CropStaff : Staff
{
    MoveCrop moveCrop = new();
    StaffWork staffWork = new();
    StaffController staffController;

    [Space(20f)]
    public BoxData boxData;
    public Transform itemBox;
    SpriteRenderer boxSpriteRenderer;
    public Transform itemBoxCover;
    SpriteRenderer coverSpriteRenderer;

    public GameObject fullSign;
    public ParticleSystem buffeffect;

    [HideInInspector] public AILerp aILerp;
    AIDestinationSetter aIDestinationSetter;

    
    [Space(10f)]
    [Header("TimerColor")]
    float count = 255;
    float num = 1;
    public float colorSpeed;
    public Image timerImage;

    [Space(10f)]
    [Header("Other")]
    public GameObject timerFinisheffect;

    public override void Awake()
    {
        base.Awake();
        staffController = GetComponentInParent<StaffController>();
        aILerp = GetComponent<AILerp>();
        aIDestinationSetter = GetComponent<AIDestinationSetter>();

        boxSpriteRenderer = itemBox.GetComponent<SpriteRenderer>();
        coverSpriteRenderer = itemBoxCover.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SetStartPos();
        staffWork.SetStart(setList, getList, waitingPos, timerImage, timerFinisheffect);
        timerImage.transform.parent.gameObject.SetActive(false);

        fullSign.SetActive(false);
    }

    private void FixedUpdate()
    {
        boxData.UpdateFullSign(fullSign, itemBox, charData.maxHandsCount);
    }

    void Update()
    {
        SetCharState();
        animDir.SetDir(aILerp.Dir, aILerp.reachedEndOfPath);

        TimerImageLevel();

        boxData.UpdateBoxSprite(animDir.lookDir, boxSpriteRenderer, coverSpriteRenderer);
        boxData.UpdateCropsPos(itemBox);

        GameManager.instance.checkList.BuffEvent(GameManager.instance.checkList.isCharSPeedBuff, buffeffect);
    }

    private void LateUpdate()
    {
        staffWork.WorkStart(aILerp.reachedEndOfPath, itemBox, charData);
        SetAnimation();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TableArea area))
        {
            if (area.transform.Equals(staffWork.target) && !moveCrop.isMoveRunning) 
            {
                moveCrop.isTouch = true;
                moveCrop.isMoveRunning = true;
                StartCoroutine(moveCrop.MoveCrops(area.tag, area, itemBox, charData.maxHandsCount, null)); 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TableArea area)) { moveCrop.isTouch = false; }
    }
  
    public override void SetCharState()
    {
        charData.SetHandsCount();
        charData.SetCoolTime();
        aILerp.speed = charData.SetMoveSpeed();
    }

    public override void SetAnimation()
    {
        if (staffWork.target != null) { aIDestinationSetter.target = staffWork.target; }

        if (aIDestinationSetter.target == null) { animDir.isMove = false; }

        if (timerImage.transform.parent.gameObject.activeSelf) { animDir.lookDir = staffController.waitingDir; }

        base.SetAnimation();
    }

    void TimerImageLevel()
    {
        timerImage.sprite = charData.SetTimerImage(charData.SetMenuType(CharData.MenuType.COOLTIME).Level);

        if (timerImage.transform.parent.gameObject.activeSelf && timerImage.fillAmount >= 1f)
        {
            if (count < 160) num = +1;
            else if (count > 255) num = -1;

            count += colorSpeed * num;

            timerImage.color = new Color(255 / 255, count / 255f, count / 255f, 255 / 255);
        }
    }
}
