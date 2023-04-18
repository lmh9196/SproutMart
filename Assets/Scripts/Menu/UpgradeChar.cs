using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeChar : MonoBehaviour
{
    public CharData charData;

    [Serializable]
    public struct Menu
    {
        public CharData.MenuType menuType;
        public GameObject upBtn;
        public GameObject maxImage;
        public Text lvText;
        public Text priceText;
    }
    public Menu[] menu;


    [Space(10f)]
    public GameObject lockCover;
    public GameObject buyCover;
    public Text buyPriceText;


    private void OnEnable() 
    {
        RenewalInfo();
    }
    private void Update()
    {
        if (lockCover != null) { lockCover.SetActive(!charData.IsAchiveTrigger); }

        if (buyCover != null)
        {
            buyCover.SetActive(!(charData.SetMenuType(CharData.MenuType.COUNT).Level > 0));
            buyPriceText.text = GameManager.instance.PriceText(charData.buyPrice,2);
        }
    }

    void RenewalInfo()
    {

        for (int i = 0; i < charData.stat.Length; i++)
        {
            for (int j = 0; j < menu.Length; j++)
            {
                if (charData.stat[i].menuType.Equals(menu[j].menuType))
                {
                    bool state = charData.stat[i].Level >= charData.stat[i].maxLevel;

                    menu[j].maxImage.SetActive(state);
                    menu[j].upBtn.SetActive(!state);


                    switch (charData.stat[i].menuType)
                    {
                        case CharData.MenuType.COUNT: menu[j].lvText.text = (charData.stat[i].Level).ToString(); break;
                        case CharData.MenuType.STACK:
                            charData.SetHandsCount();
                            menu[j].lvText.text = charData.maxHandsCount.ToString(); break;
                        case CharData.MenuType.COOLTIME:
                            charData.SetCoolTime();
                            menu[j].lvText.text = charData.coolTime.ToString(); break;
                    }

                    if (charData.stat[i].Level < charData.stat[i].price.Length)
                    {
                        menu[j].priceText.text = GameManager.instance.PriceText(charData.SetPrice(charData.stat[i]), 2);
                    }
                }
            }
        }
    }


    //public void Buy() { MenuManager.instance.BuyStaff(charData.buyPrice, staffController, charData); }
    CharData.Stat currentStat;
    public void Stack() 
    {
        currentStat = charData.SetMenuType(CharData.MenuType.STACK);
        Upgrade(); 
    } 
    public void Count() 
    {
        currentStat = charData.SetMenuType(CharData.MenuType.COUNT);
        Upgrade(); 
    } 
    public void Speed() 
    {
        currentStat = charData.SetMenuType(CharData.MenuType.COOLTIME);
        Upgrade();
    }
    void Upgrade() { MenuManager.instance.upgrade.UpgradeStat(charData.SetPrice(currentStat), levelUp); }
    void levelUp() { currentStat.Level++; RenewalInfo(); }

}
