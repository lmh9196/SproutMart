using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance = null;

    public Color textFailColor;
    bool isTextColoring;

    public Color statueDetectTriggerColor;
    bool isTriggerColoring;



    private void Awake() 
    { 
        if (instance == null) instance = this; 
        DontDestroyOnLoad(gameObject);
    }

    public void GoldFail(GameManager.GoldType goldType)
    {
        if (!isTextColoring)
        {
            switch (goldType)
            {
                case GameManager.GoldType.GOLD: StartCoroutine(TextColorAnim(GameManager.instance.goldTxt)); break;
                case GameManager.GoldType.GEM: StartCoroutine(TextColorAnim(GameManager.instance.gemTxt)); break;
                case GameManager.GoldType.BOXGOLD: StartCoroutine(TextColorAnim(GameManager.instance.boxGoldTxt)); break;
            }
        }
    }

    IEnumerator TextColorAnim(Text text)
    {
        isTextColoring = true;

        text.DOKill(true);
        text.DOColor(textFailColor, 0.5f).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(1f);

        isTextColoring = false;
    }

    public void BuildFail(List<StatueDetectSquare> detectList) { if (!isTriggerColoring){ StartCoroutine(DetectColorAnim(detectList)); } }
    IEnumerator DetectColorAnim(List<StatueDetectSquare> detectList)
    {
        isTriggerColoring = true;
        for (int i = 0; i < detectList.Count; i++) { if(detectList[i].spriteRenderer.sprite == detectList[i].red) detectList[i].spriteRenderer.DOColor(statueDetectTriggerColor, 1.0f).SetEase(Ease.Flash, 8, 0); }
        GameManager.instance.canvasList.blockCanvas.enabled = true;
        yield return new WaitForSeconds(1.1f);

        isTriggerColoring = false;
        GameManager.instance.canvasList.blockCanvas.enabled = false;
    }


}
