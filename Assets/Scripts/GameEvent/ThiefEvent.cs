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
    GameObject noticeBtn;

    public GameObject thief;

    [SerializeField] StageManager stageManager;

    bool isSpawn;

    void Start()
    {
        int rand = Random.Range(timerMinRange, timerMaxRange);
        timer = rand;   
    }

    void Update()
    {
      
        if (!thief.activeSelf)
        {
            timer -= Time.deltaTime;

            if (noticeBtn != null) { GameEventManager.instance.DisableBtn(noticeBtn.transform); }
        }
        else if (noticeBtn == null) { noticeBtn = GameEventManager.instance.SpawnRegistBtn(noticeBtnPrefabs, () => MainCamera.instance.CamEventBtn(thief), false); }
    

        if (timer < 0 && GameManager.instance.CheckTutorial(GameManager.instance.SelectGold(GameManager.GoldType.BOXGOLD) > 0)) { Spawn(); } 
    }

    public void Spawn()
    {
        GameEventManager.instance.SpawnEvent(thief.transform, stageManager.spawner.SelectSpawner());

        int rand = Random.Range(timerMinRange, timerMaxRange);
        timer = rand;
    }

}
