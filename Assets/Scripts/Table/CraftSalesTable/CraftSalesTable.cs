using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftSalesTable : Table
{
    public CraftData craftData;

    public GameObject tableObj;

    public Sprite hopeSprite;

    [Space(20f)]
    [Header("Object")]
    public SetArea[] setAreas;
    public SalesArea finishArea;
    public DetectArea detectArea;
    public Transform throwPoint;
    public Transform lookPoint;

    [Space(20f)]
    [Header("Effect")]
    public ParticleSystem enterEffect;
    public ParticleSystem exitEffect;
    public ParticleSystem workingEffect;
    public ParticleSystem speedBuffEffect;

    [Space(20f)]
    [Header("Other")]
    public ArrangementWayPoint wayPoint;
    public AudioSource audioPlayer;
    public AudioSource effectAudioPlayer;
    public Animator anim;


    public override void Awake()
    {
        base.Awake();

        UnlockInit();

        for (int i = 0; i < setAreas.Length; i++) { setAreas[i].InitTableArea(this); }

        finishArea.InitTableArea(this);

        detectArea.Init(this);

        salesWork.Init(this, finishArea, lookPoint, hopeSprite);

        craftWork.Init(setAreas, finishArea, CraftCorutine);

        wayPoint.InitWayList(saleWayList, wayPoint.wayPointParent);

        craftData.Init();

        effectAudioPlayer.transform.position = throwPoint.position;
    }

    public override void Start()
    {
        base.Start();

        wayPoint.ActiveWayPoint();

        craftData.InitNeedPrice(data.unlockNeedCount);
    }


    void Update()
    {
        UpdateUnlock(tableObj, lockArea.gameObject);

        UpdateCountText(setAreas);

        if (data.IsUnlock) { ReigstStageManager(); }


        if (craftWork.CheckArea() && data.IsUnlock) { craftWork.Work(); }
        if (salesWork.CheckArea() && data.IsUnlock) { salesWork.Work(); }


        GameManager.instance.checkList.BuffEvent(GameManager.instance.checkList.isCraftSpeedBuff, speedBuffEffect);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (transform.parent.gameObject.activeSelf && data.IsUnlock) { craftData.IsAchiveTrigger = true; }
        else { craftData.IsAchiveTrigger = false;}
    }


    public void CraftCorutine() { StartCoroutine(craftWork.Crafting(craftData, anim, throwPoint, audioPlayer, effectAudioPlayer, enterEffect, exitEffect, workingEffect)); }
}
