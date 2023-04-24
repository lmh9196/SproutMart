using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using static AdMobManager;

public class AdMobManager : MonoBehaviour
{
    public static AdMobManager instance;

    public StageData stageData;

    public Action adMobRewardAct;

    public enum Buff { NONE,CRAFT, CHAR }


    [Serializable]
    public struct ADPopUpContents
    {
        public GameObject askRemoveAD;
        public GameObject askSpeedWeek;
    }
    public ADPopUpContents popUpContents;


    [Serializable]
    public struct ADStruct
    {
        public Buff buff;

        public bool isActiving;
        public bool isHiding;

        public float currentSapwnTimer;
        public float defaultSpawnTime;
        public float currentDurationTimer;
        public float defaultDurationTime;
        
        public Button button;
    }
    public ADStruct[] adStruct;

    public struct ADCharSpeedWeek
    {
        public DateTime startTime;
        public TimeSpan timeDif;
    }
    public ADCharSpeedWeek speedWeek;

    [Space (10f)]
    [Header ("Interstitial")]
    public float adMobInterstitialTimer;
    public int InsterSpawnTimer;

    AdMobInterstitial adMobInterstitial;
    AdMobReward adMobReward;

    [HideInInspector] public float tempBGMValue;
    [HideInInspector] public float tempSFXValue;
    private void Awake()
    {
        if (null == instance) instance = this;

        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Start()
    {
        GameManager.instance.checkList.speedOn = () => { };
        GameManager.instance.checkList.speedOff = () => { };
        if (ES3.KeyExists("BuyCharSpeedWeek")) 
        { 
            GameManager.instance.checkList.IsBuyCharSpeedWeek = ES3.Load<bool>("BuyCharSpeedWeek");
            speedWeek.startTime = ES3.Load<DateTime>("SpeedWeekStartTime");
        }
    }

    void Update()
    {
        if (GameManager.instance.checkList.IsBuyCharSpeedWeek)
        {
            speedWeek.timeDif = GameManager.instance.data.currentTime - speedWeek.startTime;

            HidingStructBtn(Buff.CHAR, true);
            Debug.Log(speedWeek.timeDif);
            if (speedWeek.timeDif.Seconds > 58) 
            {
                HidingStructBtn(Buff.CHAR, false);
                GameManager.instance.checkList.IsBuyCharSpeedWeek = false; }
        }


        if (GameManager.instance.checkList.IsTutorialEnd)
        {
            AdMobInterstitialActive();
            UpdateADBtn();
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            BuySpeedAD();
        }
    }
    void Init()
    {
        adMobInterstitial = GetComponent<AdMobInterstitial>();
        adMobReward = GetComponent<AdMobReward>();
        adMobInterstitialTimer = 0;

        for (int i = 0; i < adStruct.Length; i++) 
        {
            adStruct[i].defaultSpawnTime = TimerReset(adStruct[i]);
            adStruct[i].button.gameObject.SetActive(false);
            adStruct[i].button.onClick.AddListener(adMobReward.Show);
            TimerReset(adStruct[i]); 
        }
    }

    public void AdInit()
    {
        Mute();
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    void AdMobInterstitialActive()
    {
        if (adMobInterstitialTimer < InsterSpawnTimer) { adMobInterstitialTimer += Time.deltaTime; }
        else
        {
            adMobInterstitial.AdStart();
            adMobInterstitialTimer = 0;
        }
    }
    public void Mute()
    {
        tempBGMValue = MenuManager.instance.setting.BGMSlider.value;
        tempSFXValue = MenuManager.instance.setting.SFXSlider.value;

        MenuManager.instance.setting.BGMSlider.value = -40;
        MenuManager.instance.setting.SFXSlider.value = -40;
    }

    public void AudioPlay()
    {
        MenuManager.instance.setting.BGMSlider.value = tempBGMValue;
        MenuManager.instance.setting.SFXSlider.value = tempSFXValue;
    }
    public int SetRewardGold() { return (int)(StageManager.instance.salesUnLockTableList.Count *  stageData.adWeight[stageData.Level]); }
    public int SetRewardGem() { return 10; }
    public void UpdateADBtn()
    {
        for (int i = 0; i < adStruct.Length; i++)
        {
            if (adStruct[i].isHiding) { continue; }

            if (!adStruct[i].button.gameObject.activeSelf) 
            { 
                adStruct[i].currentSapwnTimer = CurrentTimerReset(adStruct[i], adStruct[i].currentSapwnTimer, adStruct[i].defaultSpawnTime);
                adStruct[i].currentDurationTimer = 0;
            }
            else
            {
                adStruct[i].currentDurationTimer = CurrentTimerReset(adStruct[i], adStruct[i].currentDurationTimer, adStruct[i].defaultDurationTime);
                adStruct[i].currentSapwnTimer = 0;
            }

            if (adStruct[i].isActiving) 
            {
                adStruct[i].currentSapwnTimer = 0;
                adStruct[i].currentDurationTimer = 0;
            }
        }
    }
    float CurrentTimerReset(ADStruct _adStruct, float currentTimer, float maxTimer)
    {
        if (currentTimer > maxTimer) 
        {
            _adStruct.defaultSpawnTime = TimerReset(_adStruct);
            _adStruct.button.gameObject.SetActive(!_adStruct.button.gameObject.activeSelf); 
            return 0;
        }
        else { return currentTimer += Time.deltaTime; }
    }
    public float TimerReset(ADStruct _adStruct)
    {
        _adStruct.currentSapwnTimer = 0;
        _adStruct.currentDurationTimer = 0;

        return UnityEngine.Random.Range(180, 300f);
    }

    public void AddGoldAct() { adMobRewardAct = () => RewardGold(); }
    public void AddGemAct() { adMobRewardAct = () => RewardGem(); }
    public void AddCraftAct() { adMobRewardAct = () => RewardCraftSpeed(); }
    public void AddMoveAct() { adMobRewardAct = () => RewardMoveSpeed(); }

    void CompareRewardActiving(Buff buff, bool isState)
    {
        for (int i = 0; i < adStruct.Length; i++)
        {
            if (adStruct[i].buff == buff) { adStruct[i].isActiving = isState; }
        }
    }

    void HidingStructBtn(Buff buff, bool isState)
    {
        for (int i = 0; i < adStruct.Length; i++)
        {
            if (adStruct[i].buff == buff) { adStruct[i].isHiding = isState; }
        }
    }

    void RewardGold()
    {
        Vector3 cameraPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0));
        Vector3 landPos = new Vector3(cameraPos.x, cameraPos.y, 0);

        GoldPool.instance.ActiveCild(GoldPool.instance.SelectGoldParent(GoldPool.instance.parentBigPouch), landPos, GameManager.GoldType.GOLD, SetRewardGold());
    }

    void RewardGem()
    {
        Vector3 cameraPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0));
        Vector3 landPos = new Vector3(cameraPos.x, cameraPos.y, 0);

        GoldPool.instance.ActiveCild(GoldPool.instance.SelectGoldParent(GoldPool.instance.parentBigPouch), landPos, GameManager.GoldType.GEM, SetRewardGem());
    }

    public float craftBuffTime;
    void RewardCraftSpeed()
    {
        CompareRewardActiving(Buff.CRAFT, true);
        CraftData[] craftDatas = Resources.LoadAll<CraftData>("R_SO_CraftData");

        for (int i = 0; i < craftDatas.Length; i++) { craftDatas[i].buffSpeed = craftDatas[i].SetMakeSpeed() / 2; }

        StartCoroutine(CraftBuffDelay(craftDatas));

        GameManager.instance.checkList.isCraftSpeedBuff = true;
    }
    IEnumerator CraftBuffDelay(CraftData[] craftDatas)
    {
        yield return new WaitForSeconds(craftBuffTime);
        CompareRewardActiving(Buff.CRAFT, false);

        for (int i = 0; i < craftDatas.Length; i++) { craftDatas[i].buffSpeed = 0; }

        GameManager.instance.checkList.isCraftSpeedBuff = false;
    }

    public float moveSpeedBuffTime;
    public NpcData npcData;

    void RewardMoveSpeed()
    {
        CompareRewardActiving(Buff.CHAR, true);
    
        StartCoroutine(CharBuffDelay());

        GameManager.instance.checkList.isCharSpeedBuff = true;
    }
    IEnumerator CharBuffDelay()
    {
        yield return new WaitForSeconds(craftBuffTime);
        
        CompareRewardActiving(Buff.CHAR, false);
        GameManager.instance.checkList.isCharSpeedBuff = false;
        popUpContents.askSpeedWeek.SetActive(true);
    }

    public void BuyRemoveAD()
    {
        GameManager.instance.ClickVib();
        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.checkList.IsBuyRemoveAD = true;
    }

    public void BuySpeedAD()
    {
        GameManager.instance.checkList.IsBuyCharSpeedWeek = true;
        speedWeek.startTime = DateTime.Now;
        ES3.Save("SpeedWeekStartTime", speedWeek.startTime);
    }
}
