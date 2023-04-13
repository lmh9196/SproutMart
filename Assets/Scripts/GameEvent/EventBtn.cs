using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EventBtn : MonoBehaviour
{
    public Transform noticeArrow;
    public Image timerImage;
    bool isTimer;
    public bool IsTimer
    {
        get { return isTimer; }
        set { isTimer = value; }
    }
    bool isNotice;
    public bool IsNotice
    {
        get { return isNotice; }
        set
        {
            if (value) { noticeArrow.gameObject.SetActive(true); }
            else { noticeArrow.gameObject.SetActive(false); }

            isNotice = value;
        }
    }

    [HideInInspector] public float maxDuration;

    public Action timerFinishAct;

    TimerImageAct timerImageAct = new();

   
    private void Update()
    {
        timerImageAct.FillTimerImage(timerImage.transform.parent.gameObject, timerImage, IsTimer, 1 / maxDuration * Time.deltaTime, 
            (() => {
                timerFinishAct?.Invoke();
                IsTimer = false; }));
    }
    public void Init(bool isNotice, float maxDuration, Action ClickAct)
    {
        GetComponent<Button>().onClick.AddListener(() => { ClickAct?.Invoke(); });
        transform.localPosition = Vector3.zero;
        transform.SetAsFirstSibling();
        IsNotice = isNotice;
        this.maxDuration = maxDuration;
    }
}
