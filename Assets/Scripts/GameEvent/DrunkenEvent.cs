using Pathfinding;
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
    GameObject noticeBtn;

    public Image touchTimer;

    public DrunkenCustomer customer;

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
        if (!customer.gameObject.activeSelf)
        {
            timer -= Time.deltaTime;
            if (noticeBtn != null)
            {
                GameEventManager.instance.DisableBtn(noticeBtn.transform);
            }
        }
        else
        {
            if (!customer.isRight && noticeBtn == null) 
            { 
                noticeBtn = GameEventManager.instance.SpawnRegistBtn(noticeBtnPrefabs, () => MainCamera.instance.CamEventBtn(customer.gameObject),false);
                touchTimer = GameEventManager.instance.eventBtnPoints[GameEventManager.instance.FindPointIndex(noticeBtn.transform)].timer;
            }

            if (customer.isRight && noticeBtn != null)
            {
                GameEventManager.instance.DisableBtn(noticeBtn.transform);
            }
        }


        if (timer < 0 && GameManager.instance.CheckTutorial(StageManager.instance.salesUnLockTableList.Count > 1)) { Spawn(); }

        if (!GameManager.instance.checkList.IsTutorial_Drunken) { Tutorial(); }
    }

    void Tutorial()
    {
        if(MainCamera.instance.TargetCamInCheck(customer.transform, 0.2f))
        {
            VideoManager.instance.VideoPlay(guideVideoClip, 0);
            GameManager.instance.checkList.IsTutorial_Drunken = true;
        }
    }

    public void Spawn()
    {
        GameEventManager.instance.SpawnEvent(customer.transform, stageManager.spawner.SelectSpawner());

        int rand = Random.Range(timerMinRange, timerMaxRange);
        timer = rand;
    }
  
}
