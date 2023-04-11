using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftGetTable : Table
{
    public CraftData craftData;

    public GameObject tableObj;

    [Space(20f)]
    [Header("Object")]
    public SetArea[] setAreas;
    public GetArea finishArea;
    public DetectArea detectArea;
    public Transform throwPoint;

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

        craftWork.Init(setAreas, finishArea, CraftCorutine);
        craftData.Init();

    }

    public override void Start() 
    {
        base.Start();

        effectAudioPlayer.transform.position = throwPoint.position;

        craftData.InitNeedPrice(data.unlockNeedCount);

        RegistNeedTable(this);
    }

    void Update()
    {
        UpdateUnlock(tableObj, lockArea.gameObject);

        UpdateCountText(setAreas);

        if(craftWork.CheckArea() && data.IsUnlock) { craftWork.Work(); }

        GameManager.instance.checkList.BuffEvent(GameManager.instance.checkList.isCraftSpeedBuff, speedBuffEffect);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public void CraftCorutine() { StartCoroutine(craftWork.Crafting(craftData, anim, throwPoint, audioPlayer,effectAudioPlayer, enterEffect, exitEffect, workingEffect)); }
}
