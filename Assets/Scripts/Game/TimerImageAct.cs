using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerImageAct 
{
    bool isRegist;

    public GameObject timerObj;
    public Image timerImage;

    public void Init(GameObject _timerObj, Image _timerImage)
    {
        timerObj = _timerObj;
        timerImage = _timerImage;
        timerImage.fillAmount = 0;
        isRegist = true;
    }

    public void FillTimerImage(GameObject _timerObj, Image _timerImage ,bool isTimerOn, float timerSpeed, Action finishAct)
    {
        if (!isRegist) { Init(_timerObj, _timerImage);}

        if (isTimerOn)
        {
            timerObj.SetActive(true);
            timerImage.fillAmount += timerSpeed;

            if (timerImage.fillAmount  == 1) 
            {
                finishAct?.Invoke(); 
            }
        }
        else
        {
            timerObj.SetActive(false);
            timerImage.fillAmount = 0;
        }
    }

    public void ResetTimer()
    {
        timerImage.transform.parent.gameObject.SetActive(false);
        timerImage.color = new Color(255 / 255, 255 / 255f, 255 / 255f, 255 / 255);
        timerImage.fillAmount = 0;
    }
}
