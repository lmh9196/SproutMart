using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;

    [Serializable]
    public struct EventBtnPoints
    {
        public Transform eventBtnPoint;

        public Image timer;
    
        public bool btnPointFullCheck;
    }

    public EventBtnPoints[] eventBtnPoints;


    private void Awake()
    {
        if (instance == null) { instance = this; }

        DontDestroyOnLoad(gameObject);
    }
  

    public void SpawnEvent(Transform spawnTarget ,Vector3 spawnPoint)
    {
        spawnTarget.position = spawnPoint;
        spawnTarget.gameObject.SetActive(true);
    }

  
    public EventBtn SpawnEventBtn(GameObject btnPrefabs, bool isNotice, Action ClickAct, float maxDuration = 0)
    {
        for (int i = 0; i < eventBtnPoints.Length; i++)
        {
            if (!eventBtnPoints[i].btnPointFullCheck)
            {
                eventBtnPoints[i].btnPointFullCheck = true;

                GameObject btn = Instantiate(btnPrefabs.gameObject, eventBtnPoints[i].eventBtnPoint);

                EventBtn eventBtn = btn.GetComponent<EventBtn>();
                eventBtn.Init(isNotice, maxDuration, ClickAct);

                return eventBtn;
            }
        }
        return null;
    }

    public void DisableBtn(EventBtn eventBtn)
    {
        for (int i = 0; i < eventBtnPoints.Length; i++)
        {
            if (eventBtnPoints[i].eventBtnPoint == eventBtn.transform.parent) { eventBtnPoints[i].btnPointFullCheck = false; break; }
        }

        Destroy(eventBtn.gameObject);
    }


}
