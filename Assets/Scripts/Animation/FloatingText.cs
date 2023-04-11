using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText
{
    public float alphaCount;

    bool isFirst;

    float countTime;
    float timer;
    float speed;

    int tempCount;
    int minusCount;


    void StartSetting(int _targetCount)
    {
        isFirst = true;

        tempCount = _targetCount;
        alphaCount = 0;
        countTime = 0.5f;
        timer = 0;
        speed = 200;
    }

    public string FloatingString(int targetCount)
    {
        if (!isFirst) { StartSetting(targetCount); }

        if (tempCount < targetCount)
        {
            minusCount += targetCount - tempCount;
            timer = countTime;
            alphaCount = 255;
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if (alphaCount > 0) { alphaCount -= Time.deltaTime * speed; }
                else { minusCount = 0; }
            }
        }

        tempCount = targetCount;

        return GameManager.instance.PriceText(minusCount, 2); 
    }
}
