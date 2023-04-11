using I2.Loc;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Reflection;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance = null;

    [SerializeField] Player player;

    public Upgrade upgrade;
    public Setting setting;
    public Statues statues;
    public Hats hats;
    public Trash trash;
    public Expand expand;


    [Serializable]
    public class NoticeArrow
    {
        public float spacing;
        public Transform noticeArrowParent;
        Transform parentMenu;

        public List<GameObject> registNoticeBtnList;
        [HideInInspector] public List<string> noticeBtnNameList;
        public List<GameObject> tempNoticeWaitiongList;

        public void Init(Transform _parentMenu)
        {
            parentMenu = _parentMenu;
            noticeArrowParent.gameObject.SetActive(false);

            if (tempNoticeWaitiongList == null) { tempNoticeWaitiongList = new(); }
            if (noticeBtnNameList == null) { noticeBtnNameList = new(); }

            for (int i = 0; i < noticeBtnNameList.Count; i++)
            {
                for (int j = 0; j < registNoticeBtnList.Count; j++)
                {
                    if (registNoticeBtnList[j].gameObject.name == noticeBtnNameList[i]) 
                    { tempNoticeWaitiongList.Add(registNoticeBtnList[j]); break; }
                }
            }
        }
        public void UpdateNoticeCheck()
        {
            if (tempNoticeWaitiongList.Count > 0)
            {
                noticeArrowParent.gameObject.SetActive(true);

                if (tempNoticeWaitiongList[0].gameObject.activeSelf) 
                {
                    noticeArrowParent.position 
                        = new Vector2(tempNoticeWaitiongList[0].transform.position.x + spacing, tempNoticeWaitiongList[0].transform.position.y); 
                }
                else { noticeArrowParent.position = new Vector2(parentMenu.position.x + spacing, parentMenu.position.y); }
            }
            else { noticeArrowParent.gameObject.SetActive(false); }
        }
        public void ActiveNotice(GameObject targetNoticeObj) 
        {
            if(!tempNoticeWaitiongList.Contains(targetNoticeObj))
            {
                tempNoticeWaitiongList.Add(targetNoticeObj);
                noticeBtnNameList.Add(targetNoticeObj.name);
            }
        }
        public void NoticeFinishClick(GameObject eventObj)
        {
            if (tempNoticeWaitiongList.Count > 0)
            {
                for (int i = 0; i < tempNoticeWaitiongList.Count; i++)
                {
                    if (eventObj == tempNoticeWaitiongList[i]) 
                    {
                        noticeBtnNameList.Remove(eventObj.name);
                        tempNoticeWaitiongList.Remove(eventObj); return;
                    }
                }
            }
        }
    }
    public NoticeArrow noticeArrow;

    [Serializable]
    public class MainInputMenu
    {
        public Button openBtn;
        public Transform menuListParent;

        public Transform menuListCloseTarget;
        public bool isMenuListEnable;
        public Sprite menuListLineSprite;
        public Sprite menuListArrowSprite;

        public void Init()
        {
            menuListCloseTarget.transform.position = menuListParent.transform.position;
            for (int i = 0; i < menuListParent.childCount; i++) { menuListParent.GetChild(i).gameObject.SetActive(false); }
        }
    }
    public MainInputMenu mainInputMenu;

    private void Awake()
    {
        if (null == instance) instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mainInputMenu.Init();
        noticeArrow.Init(mainInputMenu.openBtn.transform);
        hats.HatInit(player.hat);
        statues.StatuesInit(player);
    }

    private void Update()
    {
        setting.UpdateVibe();
        setting.UpdateSound();
        noticeArrow.UpdateNoticeCheck();
        expand.UpdateNpcLvText();
        UpdateMenuList();
    }

    //Upgrade
    void UpdateMenuList()
    {
        for (int i = 0; i < mainInputMenu.menuListParent.childCount; i++)
        {
            if (mainInputMenu.isMenuListEnable) 
            {
                if (mainInputMenu.menuListParent.GetChild(i).position.y >= mainInputMenu.openBtn.transform.position.y + 0.5f) { mainInputMenu.menuListParent.GetChild(i).gameObject.SetActive(true); }
            }
            else
            {
                if (mainInputMenu.menuListParent.GetChild(i).position.y <= mainInputMenu.openBtn.transform.position.y + 0.5f) { mainInputMenu.menuListParent.GetChild(i).gameObject.SetActive(false); }
            }
        }
    }
    public void CallNotice(GameObject targetObj) { noticeArrow.ActiveNotice(targetObj); }
    public void FinishNotice() { noticeArrow.NoticeFinishClick(EventSystem.current.currentSelectedGameObject); }
    public void LoockAroundMenu(LookAroundCamera cam) 
    {
        if (!cam.gameObject.activeSelf) 
        { 
            cam.gameObject.SetActive(true);
            MainCamera.instance.GetComponent<AudioListener>().enabled = false;
            cam.StartPreivew();
        }
        else
        {
            cam.gameObject.SetActive(false);
            MainCamera.instance.GetComponent<AudioListener>().enabled = true;
            cam.EndPreview();
        }
        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();
    }
    public void UISortDown(GameObject page)
    {
        SoundManager.instance.PlaySfx("Page");
        GameManager.instance.ClickVib();

        page.transform.SetAsLastSibling();
    }


    //Hat
    public void TakeOffHat(HatData hatData) { hats.TakeOffHat(hatData); }

    //Setting
    public void Mute(Slider slider) { setting.Mute(slider); }
    public void ChangeBGMLeft() { setting.ChangeBGMLeft(); }
    public void ChangeBGMRight() { setting.ChangeBGMRight(); }
    public void VibActive() { setting.VibActive(); }
    public void VibUp() { setting.VibUp(); }
    public void VibDown() { setting.VibDown(); }
    public void ChangeLang(string _LanguageName) { setting.ChangeLang(_LanguageName); }

    //Other

    public void StatueChangeCategory() { statues.ChangeCategory(); }
    public void menuOnOff(GameObject menu)
    {
        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();

        if (menu.activeSelf) { menu.SetActive(false); }
        else 
        { 
            menu.SetActive(true);

            mainInputMenu.isMenuListEnable = false;
            mainInputMenu.openBtn.image.sprite = mainInputMenu.menuListLineSprite;
            mainInputMenu.menuListParent.DOLocalMoveY(mainInputMenu.menuListCloseTarget.localPosition.y, 1f).SetEase(Ease.Unset);
        }
    }
    public void JustEnable(GameObject obj) { obj.SetActive(true); }
    public void JustDisable(GameObject obj) { obj.SetActive(false); }
    public void MenuListOpen()
    {
        GameManager.instance.ClickVib();
        SoundManager.instance.PlaySfx("Pop");

        if (mainInputMenu.isMenuListEnable)
        {
            mainInputMenu.isMenuListEnable = false;
            mainInputMenu.openBtn.image.sprite = mainInputMenu.menuListLineSprite;
            mainInputMenu.menuListParent.DOLocalMoveY(mainInputMenu.menuListCloseTarget.localPosition.y, 1f).SetEase(Ease.Unset);
        }
        else
        {
            mainInputMenu.isMenuListEnable = true;
            mainInputMenu.openBtn.image.sprite = mainInputMenu.menuListArrowSprite;
            mainInputMenu.menuListParent.DOLocalMoveY(mainInputMenu.openBtn.transform.localPosition.y, 1f).SetEase(Ease.OutBack);
        }
    }
}

[Serializable]
public class Setting
{
    [Header("BGM")]
    [SerializeField] Text BGMTitle;
    [Space(10f)]
    public Slider BGMSlider;
    [SerializeField] Image iconBGM;
    [SerializeField] Sprite BGMMuteSprite;
    [SerializeField] Sprite BGMPlaySprite;
    [HideInInspector] public int BGMSelectNum;

    [Header("SFX")]
    [Space(20f)]
    public Slider SFXSlider;
    [SerializeField] Image iconSFX;
    [SerializeField] Sprite SFXMuteSprite;
    [SerializeField] Sprite SFXPlaySprite;

    [Header("Vibe")]
    [Space(20f)]
    [SerializeField] Image iconVibe;
    [SerializeField] Sprite vibPlaySprite;
    [SerializeField] Sprite vibMuteSprite;

    [SerializeField] Image vibGage;
    [SerializeField] Sprite[] vibInsImageArry;

    [HideInInspector] public int vibIntensity;
    [HideInInspector] public int vibSelectNum;

    [Space(20f)]
    string currentLang;


    public void Init()
    {
        vibSelectNum = 7;
        BGMSelectNum = 1;
        BGMSlider.value = -20f;
        SFXSlider.value = -20f;

        currentLang = LocalizationManager.CurrentLanguage;
    }
    public void UpdateVibe()
    {
        if (vibIntensity == 0) { iconVibe.sprite = vibMuteSprite; }
        else { iconVibe.sprite = vibPlaySprite; }

        vibGage.sprite = vibInsImageArry[vibSelectNum];
        vibIntensity = vibSelectNum * 10;
    }

    public void UpdateSound()
    {
        BGMTitle.text = (BGMSelectNum + 1).ToString();

        if (!SoundManager.instance.BGMAudioSorceArray[BGMSelectNum].isPlaying) 
        { SoundManager.instance.PlayBGM(BGMSelectNum); }

        UpdateSoundIcon(iconBGM, BGMMuteSprite, BGMPlaySprite, BGMSlider.value);
        UpdateSoundIcon(iconSFX, SFXMuteSprite, SFXPlaySprite, SFXSlider.value);
    }
    void UpdateSoundIcon(Image icon, Sprite muteSprite, Sprite playSprite, float value)
    {
        if (value > -40) { icon.sprite = playSprite; }
        else { icon.sprite = muteSprite; }
    }


    public void Mute(Slider slider) { slider.value = -40; GameManager.instance.ClickVib(); }
    public void ChangeBGMLeft()
    {
        if (BGMSelectNum > 0) { BGMSelectNum--; }
        else { BGMSelectNum = 2; }
        GameManager.instance.ClickVib();
    }
    public void ChangeBGMRight()
    {
        if (BGMSelectNum < 2) { BGMSelectNum++; }
        else { BGMSelectNum = 0; }
        GameManager.instance.ClickVib();
    }

    int tempVibValue;
    public void VibActive()
    {
        if (vibSelectNum > 0)
        {
            tempVibValue = vibSelectNum;
            vibSelectNum = 0;
        }
        else
        {
            vibSelectNum = tempVibValue;
            GameManager.instance.ClickVib();
        }
    }
    public void VibUp()
    {
        if (vibSelectNum < 7) { vibSelectNum++; }
        vibIntensity = vibSelectNum * 10;
        GameManager.instance.ClickVib();
    }
    public void VibDown()
    {
        if (vibSelectNum > 0) {vibSelectNum--; }
        vibIntensity = vibSelectNum * 10;
        GameManager.instance.ClickVib();
    }

    public void ChangeLang(string LanguageName)
    {
        if (LocalizationManager.HasLanguage(LanguageName)) 
        { 
            LocalizationManager.CurrentLanguage = LanguageName;
            currentLang = LocalizationManager.CurrentLanguage;
        }
    }

    public void SaveData()
    {
        Dictionary<string, SettingSaveData> settingDataDic = new();

        settingDataDic.Add("SettingManager", new SettingSaveData(BGMSelectNum, vibSelectNum, BGMSlider.value, SFXSlider.value, currentLang));
        ES3.Save("SettingManager", settingDataDic);
    }

    public void LoadData()
    {
        if(ES3.KeyExists("SettingManager"))
        {
            Dictionary<string, SettingSaveData> settingDic = new();

            settingDic = ES3.Load("SettingManager", settingDic);
            BGMSelectNum = settingDic["SettingManager"].BGMselectNum;
            vibSelectNum = settingDic["SettingManager"].vibSelectNum;
            BGMSlider.value = settingDic["SettingManager"].BGMSliderValue;
            SFXSlider.value = settingDic["SettingManager"].SFXSliderValue;
            LocalizationManager.CurrentLanguage = settingDic["SettingManager"].currentLang;
        }
    }

    public class SettingSaveData
    {
        public int BGMselectNum;
        public int vibSelectNum;
        public float BGMSliderValue;
        public float SFXSliderValue;
        public string currentLang;
        public SettingSaveData(int BGMselectNum, int vibSelectNum, float BGMSliderValue, float SFXSliderValue, string currentLang)
        {
            this.BGMselectNum = BGMselectNum;
            this.vibSelectNum = vibSelectNum;
            this.BGMSliderValue = BGMSliderValue;
            this.SFXSliderValue = SFXSliderValue;
            this.currentLang = currentLang;
        }
    }
}
[Serializable]
public class Upgrade
{
    public Transform staffPage;
    public Transform machinePage;

    public void UpgradeStat(int price, Action levelUp)
    {
        if (GameManager.instance.CompareGold(price,GameManager.instance.SelectGold(GameManager.GoldType.GOLD), true))
        {
            levelUp?.Invoke();
            GameManager.instance.CalGold(GameManager.GoldType.GOLD, -price);
            SoundManager.instance.PlaySfx("Buy");
            GameManager.instance.ClickVib();
        }
        else
        {
            SoundManager.instance.PlaySfx("Fail");
            GameManager.instance.ClickVib();

            ColorManager.instance.GoldFail(GameManager.GoldType.GOLD);
        }
    }
}
[Serializable]
public class Statues
{
    Player player;
    public StatueManager statueManager;

    public Button moveBtn;
    public Button destroyBtn;
    public Button activeBtn;
    public Button cancleBtn;


    int selectBtnIdx;
    public int SelectBtnIndx
    {
        get { return selectBtnIdx; }
        set
        {
            selectBtnIdx = value;

            statueCategories[value].categoryBtn.enabled = false;
            statueCategories[value].categoryBtn.image.sprite = statueCategories[value].selectImage;
            statueCategories[value].categoryBtn.transform.position = statueCategories[value].defalutPos;
            for (int i = 0; i < statueCategories.Length; i++)
            {
                if(i != value)
                {
                    statueCategories[i].categoryBtn.enabled = true;
                    statueCategories[i].categoryBtn.image.sprite = statueCategories[i].normalImage;
                    statueCategories[i].categoryBtn.transform.localPosition = new Vector3(statueCategories[i].defalutPos.x , statueCategories[i].defalutPos.y - 9.5f, 0);
                }
            }
        }
    }

    [Serializable]
    public struct StatueCategory
    {
        public Button categoryBtn;
        public Sprite selectImage;
        public Sprite normalImage;
        public Vector3 defalutPos;
    }
    public StatueCategory[] statueCategories;


    public void ChangeCategory()
    {
        GameManager.instance.ClickVib();
        SoundManager.instance.PlaySfx("Pop");
        for (int i = 0; i < statueCategories.Length; i++)
        {
            if(EventSystem.current.currentSelectedGameObject == statueCategories[i].categoryBtn.gameObject)
            {
                SelectBtnIndx = i; break;
            }
        }
    }
  

    public void StatuesInit(Player _player)
    { 
        player = _player;

        for (int i = 0; i < statueCategories.Length; i++)
        {
            statueCategories[i].defalutPos =  statueCategories[i].categoryBtn.transform.position;
        }


        SelectBtnIndx = 0;

        moveBtn.onClick.AddListener(MoveStatue);
        destroyBtn.onClick.AddListener(DestroyStatue);
        activeBtn.onClick.AddListener(ActiveStatue);
        cancleBtn.onClick.AddListener(CancleStatue);

    }

    public void BuyStatue(Statue statue)
    {
        GameManager.instance.ClickVib();

        if (statueManager.EnableCheck(StatueManager.BuildMode.BUILD, statue))
        {
            SoundManager.instance.PlaySfx("Pop");
            statueManager.InitStatueManager(StatueManager.BuildMode.BUILD, statue);
        }
        else 
        {
            //DialogueManager.instance.EnableDialouge(DialogueManager.instance.term.term_NotEnoughGold);
            SoundManager.instance.PlaySfx("Fail");
        }
    }
    void MoveStatue()
    {
        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();

        statueManager.InitStatueManager(StatueManager.BuildMode.MOVE);
    }
    void DestroyStatue()
    {
        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();

        statueManager.InitStatueManager(StatueManager.BuildMode.DESTROY);
    }
    void ActiveStatue()
    {
        GameManager.instance.ClickVib();

        if (statueManager.EnableCheck(statueManager.buildMode, statueManager.selectStatue)) { statueManager.FinishAct(); }
        else { SoundManager.instance.PlaySfx("Fail"); }
    }


    void CancleStatue()
    {
        GameManager.instance.ClickVib();
        SoundManager.instance.PlaySfx("Pop");
        statueManager.CancleAct();
    }
}
[Serializable]
public class Hats
{
    Hat playerHat;

    public Sprite goldCoinImage;
    public Sprite gemCoinImage;

    public Sprite InitCoinImage(GameManager.GoldType goldType)
    {
        switch (goldType)
        {
            case GameManager.GoldType.GOLD: return goldCoinImage;
            case GameManager.GoldType.GEM: return gemCoinImage;
            default: return null;
        }
    }
    public void AddHatActiveButton(Button buyButton, HatData hatData) { buyButton.onClick.AddListener(() => CheckHatBtn(hatData)); }
    public void HatInit(Hat _playerHat) { playerHat = _playerHat; }
    public void CheckHatBtn(HatData currentHatData)
    {
        if (currentHatData.isBought) ActiveHat(currentHatData);
        else BuyHat(currentHatData);
    }

    void BuyHat(HatData hatData)
    {
        if (GameManager.instance.CompareGold(hatData.HatPrice(), GameManager.instance.SelectGold(hatData.goldType), true))
        {
            SoundManager.instance.PlaySfx("Buy");
            GameManager.instance.ClickVib();

            GameManager.instance.CalGold(hatData.goldType, - hatData.HatPrice());

            hatData.RegistHatData(playerHat);
            hatData.isBought = true;
        }
        else
        {
            SoundManager.instance.PlaySfx("Fail");
            GameManager.instance.ClickVib();

            ColorManager.instance.GoldFail(hatData.goldType);
        }
    }
    void ActiveHat(HatData hatData)
    {
        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();

        hatData.RegistHatData(playerHat);
    }

    public void TakeOffHat(HatData hatData)
    {
        SoundManager.instance.PlaySfx("Pop");
        GameManager.instance.ClickVib();

        hatData.RegistHatData(playerHat);
    }

}

[Serializable]
public class Trash
{
    public Button trashBtn;
}
[Serializable]

public class Expand
{
    public NpcData npcData;
    public Text npcLvText;

    public void UpdateNpcLvText()
    {
        npcLvText.text = npcData.level.ToString();
    }
}
