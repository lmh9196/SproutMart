using DG.Tweening;
using RDG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class Game_Data
{
    public int gold;
    public int gem;
    public int boxGold;

    public string saveSceneName;
}

[Serializable]
public class CheckList
{
    [Space(10f)]
    bool isTutorialEnd;
    public bool IsTutorialEnd { get { return isTutorialEnd; }
        set
        {
            isTutorialEnd = value;
            ES3.Save("Tutorial", isTutorialEnd);
        }
    }

    bool isTutorial_Drunken;
    public bool IsTutorial_Drunken
    {
        get { return isTutorial_Drunken; }
        set
        {
            isTutorial_Drunken = value;
            ES3.Save("Tutorial_Drunken", isTutorial_Drunken);
        }
    }

    bool isTutorial_Full;
    public bool IsTutorial_Full
    {
        get { return isTutorial_Full; }
        set
        {
            isTutorial_Full = value;
            ES3.Save("Tutorial_Full", isTutorial_Full);
        }
    }

    public bool isBuildMode;
    public bool isCamEvent;
    public bool isMenuOpen;
    public bool isCharSpeedBuff;
    public bool isCraftSpeedBuff;
    public bool isLookAround;
    public bool isItemBoxOff;

    public bool CheckDoubleTabZoom()
    {
        if (isTutorialEnd && !isBuildMode && !isMenuOpen) { return true; }
        else { return false; }
    }
    public void BuffEvent(bool isCheck, ParticleSystem buffeffect)
    {
        if(buffeffect != null)
        {
            if (isCheck) { if (!buffeffect.isPlaying) { buffeffect.Play(); } }
            else { if (buffeffect.isPlaying) { buffeffect.Stop(); } }
        }
    }
}
[Serializable]
public class CanvasList
{
    public Transform UiParent;
    public Canvas joyCanvas;
    public Canvas mainCanvas;
    public Canvas inputCanvas;
    public Canvas burrowCanvas;
    public Canvas menuCanvas;
    public Canvas lookCanvas;
    public Canvas statueCanvas;
    public Canvas videoCanvas;
    public Canvas blockCanvas;
    public Canvas loadingCanvas;

    public List<CanvasScaler> matchCanvasScalerList = new();
    public void Init() 
    { 
        blockCanvas.enabled = false;
        loadingCanvas.enabled = false;
        joyCanvas.enabled = true;
        inputCanvas.enabled = true;
        mainCanvas.enabled = true;
        burrowCanvas.enabled = true;
        menuCanvas.enabled = true;
        videoCanvas.enabled = true;
    }

    public bool UpdateMenuOpenCheck()
    {
        for (int i = 0; i < menuCanvas.transform.childCount; i++)
        {
            if (menuCanvas.transform.GetChild(i).gameObject.activeSelf) { return true; }
        }
        return false;
    }
    public void ActiveJoystickCanvas(bool isState) { joyCanvas.gameObject.SetActive(isState); }
    public void ActiveBurrowCanvas(bool isState)
    {
        menuCanvas.enabled = !isState;
        inputCanvas.GetComponent<GraphicRaycaster>().enabled = !isState;
    }
    public void ActiveLookAroundCanvas(bool isState) 
    { 
        lookCanvas.enabled = isState;
        menuCanvas.enabled = !isState;
        joyCanvas.enabled = !isState;
        inputCanvas.enabled = !isState;
        burrowCanvas.enabled = !isState;
        mainCanvas.enabled = !isState;
    }
    public void ActiveCameraEventCanvas(bool isState)
    {
        menuCanvas.enabled = !isState;
        joyCanvas.enabled = !isState;
        inputCanvas.enabled = !isState;
        burrowCanvas.enabled = !isState;
        mainCanvas.enabled = !isState;
    }
    public void ActiveTutorialCanvas(bool isState)
    {
        menuCanvas.enabled = !isState;
        inputCanvas.enabled = !isState;
        burrowCanvas.enabled = !isState;
        mainCanvas.enabled = !isState;
    }
    public void BuildModeCanvas(bool isState)
    {
        statueCanvas.enabled = isState;

        menuCanvas.enabled = !isState;
        joyCanvas.enabled = !isState;
        inputCanvas.enabled = !isState;
        burrowCanvas.enabled = !isState;
    }
    public void ActiveLoadingCanvas()
    {
        Canvas[] allUI = UiParent.GetComponentsInChildren<Canvas>(true);

        for (int i = 0; i < allUI.Length; i++) { allUI[i].enabled = false; }
      
        loadingCanvas.enabled = true;
    }
}

[Serializable]

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Game_Data data;
    public CheckList checkList;
    public CanvasList canvasList;


    public enum GoldType { GOLD, GEM, BOXGOLD}

    [Header("GameInfo")]
    public Text goldTxt;
    public Sprite goldSprite;
    public Text gemTxt;
    public Sprite gemSprite;
    public Text boxGoldTxt;

    public Color textFailColor;

    DialogueTerm term = new();
    private void Awake()
    {
        if (null == instance) 
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
        else { Destroy(this); }
        DontDestroyOnLoad(canvasList.UiParent.gameObject);

        Screen.SetResolution(1080, 1920, true);
        Application.targetFrameRate = 60;

        Screen.orientation = ScreenOrientation.Portrait;//게임 시작시 세로고정
        Screen.orientation = ScreenOrientation.AutoRotation; 
    }

    bool isCheckStart;
    bool isCheckDone;
    IEnumerator CheckTouch() //테스트용 튜토리얼 스킵
    {
        isCheckStart = true;

        float touchCount = 0;
        float timer = 0;

        while (true)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f) { break; }
            else 
            {
                if (Input.GetMouseButtonDown(0))
                {
                    timer = 0;
                    touchCount++;
                }
            }
            if (touchCount > 4) {
                StartCoroutine(Tutorial.instance.End()); 
                isCheckDone = true; break; }
            yield return null;
        }

        isCheckStart = false;
    }
    private void Update()
    {
        //테스트용
        if (Input.GetKeyDown(KeyCode.Z)) { Player.instance.Booster(); } 
        if (!isCheckStart && !isCheckDone 
            && !checkList.IsTutorialEnd &&
            Input.GetMouseButtonDown(0)) { StartCoroutine(CheckTouch()); }


        checkList.isMenuOpen = canvasList.UpdateMenuOpenCheck();

        goldTxt.text = PriceText(data.gold,2);
        gemTxt.text = PriceText(data.gem,2);
        boxGoldTxt.text = PriceText(data.boxGold,2);
    }

    public void ClickVib(int time = 50, int strong= 1) 
    {
        if (MenuManager.instance.setting.vibSelectNum > 0) { Vibration.Vibrate(time, MenuManager.instance.setting.vibIntensity * strong); }
        else { return; }
    }

    public Tuple<int, int, int> CalKPrice(int _num)
    {
        return new Tuple<int, int, int>( _num / 1000, (_num % 1000)/100, (_num % 100)/10);
    }

    public string PriceText(int num, int digit)
    {
        if (CalKPrice(num).Item1 > 0)
        {
            if (digit == 0) { return CalKPrice(num).Item1 + "k"; }
            else if (digit == 1)
            {
                if (CalKPrice(num).Item2 > 0) { return CalKPrice(num).Item1 + "." + CalKPrice(num).Item2 + "K"; }
                else { return CalKPrice(num).Item1 + "K"; }
            }
            else if (digit == 2)
            {
                if (CalKPrice(num).Item3 > 0) { return CalKPrice(num).Item1 + "." + CalKPrice(num).Item2 + CalKPrice(num).Item3 + "K"; }
                else if (CalKPrice(num).Item2 > 0) { return CalKPrice(num).Item1 + "." + CalKPrice(num).Item2 + "K"; }
                else { return CalKPrice(num).Item1 + "K"; }
            }
            else { return "Error : Digit"; }
        }
        else return num.ToString();
    }

    public void AstarScanDelay() { StartCoroutine(AstarScan()); }
    IEnumerator AstarScan() 
    {
        yield return new WaitForSeconds(1f);
        AstarPath.active.Scan();
    }

    public void CalGold(GoldType goldType, int inputGold)
    {
        switch(goldType)
        {
            case GoldType.GOLD: data.gold += inputGold; break;
            case GoldType.GEM: data.gem += inputGold; break;
            case GoldType.BOXGOLD: data.boxGold += inputGold; break;
        }
    }
    public bool CompareGold(int targetGold, int gameGold, bool isGuide)
    {
        if (targetGold <= gameGold) { return true; }
        else 
        {
            if (isGuide) { DialogueManager.instance.GuideFlashCoroutine(term.term_NotEnoughGold); }
            return false; 
        }
    }
    public int SelectGold(GoldType goldType)
    {
        switch (goldType)
        {
            case GoldType.GOLD: return data.gold;
            case GoldType.GEM: return data.gem;
            case GoldType.BOXGOLD: return data.boxGold;
        }
        return 0;
    }

  
 

    

    public void AssignGold(GoldType goldType, int inputGold)
    {
        switch (goldType)
        {
            case GoldType.GOLD: data.gold = inputGold; break;
            case GoldType.GEM: data.gem = inputGold; break;
            case GoldType.BOXGOLD: data.boxGold = inputGold; break;
        }
    }

    public int PointerIDCheck() //터치 기기 체크
    {

        int pointerID = 0;
#if UNITY_EDITOR

        pointerID = - 1;

#elif UNITY_IOS || UNITY_IPHONE

        pointerID =  0;

#endif

        return pointerID;
    }

}
