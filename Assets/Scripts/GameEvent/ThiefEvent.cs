using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThiefEvent : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] int timerMinRange;
    [SerializeField] int timerMaxRange;

    public GameObject noticeBtnPrefabs;
    [HideInInspector] public EventBtn thiefEventBtn;

    public GameObject thief;

    [SerializeField] StageManager stageManager;

    bool thiefState;
    public bool ThiefState
    {
        get { return thiefState; }
        set
        {
            if (thiefState && !value) { GameEventManager.instance.DisableBtn(thiefEventBtn); }

            thiefState = value;
        }
    }
    void Start()
    {
        int rand = Random.Range(timerMinRange, timerMaxRange);
        timer = rand;   
    }

    void Update()
    {
        ThiefState = thief.gameObject.activeSelf;

        if (!ThiefState)
        {
            timer -= Time.deltaTime;

            if (timer < 0 && GameManager.instance.checkList.IsTutorialEnd 
                && GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD)>0) { Spawn(); }
        }
    }

    public void Spawn()
    {
        GameEventManager.instance.SpawnEvent(thief.transform, stageManager.spawner.SelectSpawner());
        thiefEventBtn = GameEventManager.instance.SpawnEventBtn(noticeBtnPrefabs, false, (() => { MainCamera.instance.CamEventBtn(thief.gameObject); }));

        int rand = Random.Range(timerMinRange, timerMaxRange);
        timer = rand;
    }

}
