using Pathfinding;
using Pathfinding.Ionic.Zlib;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DrunkenEvent : MonoBehaviour
{
    [SerializeField] float timer;
    public float maxDurationTime;

    [SerializeField] int timerMinRange;
    [SerializeField] int timerMaxRange;


    public GameObject noticeBtnPrefabs;
    [HideInInspector] public EventBtn drunkenEventBtn;

    public DrunkenCustomer customer;
    bool customerState;
    public bool CustomerState
    {
        get { return customerState; }
        set 
        {
            if (customerState && !value) { GameEventManager.instance.DisableBtn(drunkenEventBtn); }
        
            customerState = value;
        }
    }

    [Space(10f)]
    [Header("Tutorial")]
    [SerializeField] VideoClip guideVideoClip;

    [SerializeField] StageManager stageManager;


    void Start()
    {
        int rand = Random.Range(timerMinRange, timerMaxRange);
        timer = rand;
    }
    void Update()
    {
        CustomerState = customer.gameObject.activeSelf;

        if (GameManager.instance.checkList.IsTutorialEnd && !CustomerState)
        {
            timer -= Time.deltaTime;

            if (timer < 0) { Spawn(); }
        }

        if (!GameManager.instance.checkList.IsTutorial_Drunken) { Tutorial(); }
    }

    void Tutorial()
    {
        if(MainCamera.instance.TargetCamInCheck(customer.transform, 0.2f))
        {
            VideoManager.instance.VideoPlay(guideVideoClip, 0);
            GameManager.instance.checkList.IsTutorial_Drunken = true;
            drunkenEventBtn.IsNotice = false;
        }
    }

    public void Spawn()
    {
        GameEventManager.instance.SpawnEvent(customer.transform, stageManager.spawner.SelectSpawner());
        drunkenEventBtn =  GameEventManager.instance.SpawnEventBtn(noticeBtnPrefabs, !GameManager.instance.checkList.IsTutorial_Drunken, (()=> { MainCamera.instance.CamEventBtn(customer.gameObject); }),maxDurationTime);
        drunkenEventBtn.timerFinishAct = customer.ReturnWorngTarget;

        int rand = Random.Range(timerMinRange, timerMaxRange);
        timer = rand;
    }
}
