using System;
using System.Collections;
using System.Collections.Generic;
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
        public GameObject noticeObj;
        public Image timer;
        public bool btnPointFullCheck;
    }
    public EventBtnPoints[] eventBtnPoints;


    private void Awake()
    {
        if (instance == null) { instance = this; }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        NoticeDisable();
    }

    public void SpawnEvent(Transform spawnTarget ,Vector3 spawnPoint)
    {
        spawnTarget.position = spawnPoint;
        spawnTarget.gameObject.SetActive(true);
    }

    public GameObject SpawnRegistBtn(GameObject btnPrefabs, Action btnAddContent, bool isNotice)
    {
        for (int i = 0; i < eventBtnPoints.Length; i++)
        {
            if (!eventBtnPoints[i].btnPointFullCheck)
            {
                GameObject btn = Instantiate(btnPrefabs.gameObject, eventBtnPoints[i].eventBtnPoint);
                btn.transform.position = eventBtnPoints[i].eventBtnPoint.position;

                btn.transform.SetAsFirstSibling();

                btn.GetComponent<Button>().onClick.AddListener(() => btnAddContent());

                eventBtnPoints[i].btnPointFullCheck = true;

                if (isNotice) { eventBtnPoints[i].noticeObj.SetActive(true); }

                return btn;
            }
        }
        return null;
    }

    public void DisableBtn(Transform noticeBtn)
    {
        eventBtnPoints[FindPointIndex(noticeBtn)].btnPointFullCheck = false;

        Destroy(noticeBtn.gameObject);
    }


    public void NoticeDisable()
    {
        if(EventSystem.current.currentSelectedGameObject != null)
        {
            eventBtnPoints[FindPointIndex(EventSystem.current.currentSelectedGameObject.transform)].noticeObj.SetActive(false);
        }
    }

    public int FindPointIndex(Transform noticeBtn)
    {
        for (int i = 0; i < eventBtnPoints.Length; i++)
        {
            if (eventBtnPoints[i].eventBtnPoint == noticeBtn.parent) { return i; }
        }

        return 0;
    }
}
