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

    public NpcData npcData;

    public Dictionary<string, Game_Data> dicSaveGameData;
    public Game_Data(int gold, int gem, int boxGold)
    {
        this.gold = gold;
        this.gem = gem;
        this.boxGold = boxGold;
    }
}

[Serializable]
public class CheckList
{
    public Action<string, bool> RegistSaveDic;

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
    public bool isCharSPeedBuff;
    public bool isCraftSpeedBuff;
    public bool isLookAround;

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
    public List<CanvasScaler> MatchCanvasList()
    {
        List<CanvasScaler> canvasList = new();

        canvasList.Add(mainCanvas.GetComponent<CanvasScaler>());
        canvasList.Add(inputCanvas.GetComponent<CanvasScaler>());
        canvasList.Add(burrowCanvas.GetComponent<CanvasScaler>());
        canvasList.Add(menuCanvas.GetComponent<CanvasScaler>());
        canvasList.Add(lookCanvas.GetComponent<CanvasScaler>());
        canvasList.Add(statueCanvas.GetComponent<CanvasScaler>());
        canvasList.Add(videoCanvas.GetComponent<CanvasScaler>());
        canvasList.Add(loadingCanvas.GetComponent<CanvasScaler>());

        return canvasList;
    }

    public bool UpdateMenuOpenCheck()
    {
        for (int i = 0; i < menuCanvas.transform.childCount; i++)
        {
            if (menuCanvas.transform.GetChild(i).gameObject.activeSelf) { return true; }
        }
        return false;
    }
    public void SetJoystick(bool isState) { joyCanvas.gameObject.SetActive(isState); }
    public void SetBurrowCanvas(bool isState)
    {
        menuCanvas.enabled = isState;
        inputCanvas.GetComponent<GraphicRaycaster>().enabled = isState;
    }
    public void SetLookAround(bool isState) 
    { 
        lookCanvas.enabled = isState;

        menuCanvas.enabled = !isState;
        joyCanvas.enabled = !isState;
        inputCanvas.enabled = !isState;
        burrowCanvas.enabled = !isState;
        mainCanvas.enabled = !isState;
    }
    public void SetCameraEvent(bool isState)
    {
        menuCanvas.enabled = isState;
        joyCanvas.enabled = isState;
        inputCanvas.enabled = isState;
        burrowCanvas.enabled = isState;
        mainCanvas.enabled = isState;
    }
    public void SetTutorial(bool isState)
    {
        menuCanvas.enabled = isState;
        inputCanvas.enabled = isState;
        burrowCanvas.enabled = isState;
        mainCanvas.enabled = isState;
    }
    public void BuildModeCanvas(bool isState)
    {
        statueCanvas.enabled = isState;

        menuCanvas.enabled = !isState;
        joyCanvas.enabled = !isState;
        inputCanvas.enabled = !isState;
        burrowCanvas.enabled = !isState;
    }
    public void SetLoadingCanvas()
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

    public Action SceneAct;


    [Header("GameInfo")]
    public Text goldTxt;
    public Sprite goldSprite;
    public Text gemTxt;
    public Sprite gemSprite;
    public Text boxGoldTxt;

    public Color textFailColor;

    [Space(10f)]
    public bool isItemBoxOff;



    //Tutorial
    public CharData casheirData;
    public GameObject upgradeMneu;

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

        Screen.orientation = ScreenOrientation.Portrait;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

  
    private void Update()
    {
        checkList.isMenuOpen = canvasList.UpdateMenuOpenCheck();

        goldTxt.text = PriceText(data.gold,2);
        gemTxt.text = PriceText(data.gem,2);
        boxGoldTxt.text = PriceText(data.boxGold,2);
    }

    public bool CheckTutorial(bool condition = true)
    {
        if (checkList.IsTutorialEnd)
        {
            if (condition) return true;
            else return false;
        }
        else return false;
    }

    public void ClickVib(int time = 50, int strong= 1) 
    {
        if (MenuManager.instance.setting.vibSelectNum > 0) { Vibration.Vibrate(time, MenuManager.instance.setting.vibIntensity * strong); }
        else { return; }
    }


    public bool ConditionCheck(bool condition ,bool isReverse)
    {
        if (!isReverse) return condition;
        else return !condition;
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
            if (isGuide) { DialogueManager.instance.GuideTrigger(DialogueManager.instance.term.term_NotEnoughGold); }
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

    public int PointerIDCheck()
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
