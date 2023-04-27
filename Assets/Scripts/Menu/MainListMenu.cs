using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

[Serializable]
public class MainListMenu
{
    public Button openBtn;
    public RectTransform menuListParent;
    public RectTransform menuListCloseTarget;

    [Space(10f)]
    bool isMenuListEnable;
    public bool IsMenuListEnable
    {
        get { return isMenuListEnable; }
        set
        {
            if(isMenuListEnable != value)
            {
                Action act = value ?
                    InitUpDownAct(openBtn.GetComponent<RectTransform>(), menuListArrowSprite, Ease.OutBack) :
                    InitUpDownAct(menuListCloseTarget, menuListLineSprite, Ease.Unset);

                act?.Invoke();
            }

            isMenuListEnable = value;
        }
    }


    [Space(10f)]
    [SerializeField] Sprite menuListLineSprite;
    [SerializeField] Sprite menuListArrowSprite;

    [Space(10f)]
    //MenuListContents
    public Button settingBtn;
    public Button upgradeBtn;
    public Button cashBtn;
    public Button storeBtn;
    public Button statueBtn;
    public Button trashBtn;

    [HideInInspector] public List<Button> listBtn = new();


    [Space(10f)]
    public float sortSpacing;

    public void Init()
    {
        DialogueTerm term = new();


        openBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySfx("Page");
            GameManager.instance.ClickVib();
            IsMenuListEnable = !IsMenuListEnable;
        });

        //Order
        CreateBtn(listBtn, settingBtn, MenuManager.instance.setting.menu);
        CreateBtn(listBtn, upgradeBtn, MenuManager.instance.upgrade.menu, () => 
        {
            if (!GameManager.instance.checkList.IsTutorial_UpgradeMenu)
            {
                GameManager.instance.checkList.IsTutorial_UpgradeMenu = true;
                DialogueManager.instance.DisableDialogue(term.termUpgrade_GuideUpgradeMenu);
                DisableNotice();
            }
        });
        CreateBtn(listBtn, cashBtn);
        CreateBtn(listBtn, storeBtn, MenuManager.instance.hats.menu);
        CreateBtn(listBtn, statueBtn, MenuManager.instance.statues.menu);

        SetParentPos(listBtn);
        SortBtnPos(listBtn);
    }

    Action InitUpDownAct(RectTransform targetRect, Sprite openBtnSprite, Ease ease)
    {
        return () =>
        {
            openBtn.image.sprite = openBtnSprite;
            menuListParent.DOLocalMoveY(targetRect.localPosition.y, 1f).SetEase(ease);
        };
    }

    void CreateBtn(List<Button> listBtn, Button btn, RectTransform connectMenu = null, Action act1 = null)
    {
        Button menuBtn = UnityEngine.Object.Instantiate(btn, menuListParent);
        menuBtn.name = btn.name;
        menuBtn.gameObject.SetActive(false);

        if (connectMenu != null) 
        {
            menuBtn.onClick.AddListener(() => { MenuManager.instance.menuOnOff(connectMenu.gameObject); });

            if (act1 != null) { menuBtn.onClick.AddListener(() => { act1?.Invoke(); }); }
        }

        listBtn.Add(menuBtn);
    }
    void SetParentPos(List<Button> listBtn)
    {
        RectTransform standardRect = openBtn.GetComponent<RectTransform>();

        float totalBtnHeigt = 0;

        for (int i = 0; i < listBtn.Count; i++)
        {
            totalBtnHeigt += sortSpacing;
            totalBtnHeigt += listBtn[i].GetComponent<RectTransform>().sizeDelta.y;
        }

        menuListParent.localPosition = new Vector3(standardRect.localPosition.x, standardRect.localPosition.y - totalBtnHeigt, 0);

        menuListCloseTarget.position = menuListParent.position;
    }
    void SortBtnPos(List<Button> listBtn)
    {
        for (int i = listBtn.Count - 1; i >= 0; i--)
        {
            RectTransform btn = listBtn[i].GetComponent<RectTransform>();

            btn.localPosition = new Vector3(0, sortSpacing * (listBtn.Count - i), 0);
        }
    }

    public Button FindBtn(Button hopeBtn )
    {
        for (int i = 0; i < listBtn.Count; i++)
        {
            if (listBtn[i].name == hopeBtn.name) { return listBtn[i]; }
        }

        Debug.Log("Error : Not Find Btn");
        return null;
    }

    public float arrowSpacing;
    public Transform noticeArrowParent;

    public List<Button> tempNoticeWaitiongList;

    public void UpdateNoticeCheck()
    {
        if (tempNoticeWaitiongList.Count > 0)
        {
            noticeArrowParent.gameObject.SetActive(true);

            if (tempNoticeWaitiongList[0].gameObject.activeSelf)
            {
                noticeArrowParent.position
                    = new Vector2(tempNoticeWaitiongList[0].transform.position.x + arrowSpacing, tempNoticeWaitiongList[0].transform.position.y);
            }
            else { noticeArrowParent.position = new Vector2(openBtn.transform.position.x + arrowSpacing, openBtn.transform.position.y); }
        }
        else { noticeArrowParent.gameObject.SetActive(false); }
    }
    public void EnableNotice(Button targetBtn)
    {
        tempNoticeWaitiongList.Add(targetBtn);
    }
    public void DisableNotice(Action act = null)
    {
        if(tempNoticeWaitiongList.Count > 0)
        {
            if (EventSystem.current.currentSelectedGameObject && tempNoticeWaitiongList[0].gameObject)
            {
                tempNoticeWaitiongList.RemoveAt(0);
                noticeArrowParent.gameObject.SetActive(false);
                act?.Invoke();
            }
        }
    }

}
