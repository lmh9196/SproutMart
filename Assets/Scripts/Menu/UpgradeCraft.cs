using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCraft : MonoBehaviour
{
   
    public CraftData craftData;

    [Serializable]
    public struct Menu
    {
        public CraftData.MenuType menuType;
        public GameObject upBtn;
        public GameObject maxImage;
        public Text lvText;
        public Text priceText;
    }
    public Menu[] menu;

    [Space(10f)]
    public GameObject lockCover;



    private void Start()
    {
        RenewalInfo();
    }

    private void Update()
    {
        if (lockCover != null ) { lockCover.SetActive(!craftData.IsAchiveTrigger); }

        for (int i = 0; i < craftData.stat.Length; i++)
        {
            GameManager.instance.TextColorInGameGold(menu[i].priceText, GameManager.GoldType.GOLD, craftData.GetPrice(craftData.stat[i]));
            menu[i].priceText.text = GameManager.instance.PriceText(craftData.GetPrice(craftData.stat[i]), 2);
        }
    }



    void RenewalInfo()
    {
        for (int i = 0; i < craftData.stat.Length; i++)
        {
            bool state = craftData.stat[i].Level >= craftData.stat[i].maxLevel;

            menu[i].maxImage.SetActive(state);
            menu[i].upBtn.SetActive(!state);

            menu[i].lvText.text = craftData.SetMakeSpeed().ToString();
        }
    }


    CraftData.Stat currentStat;
    public void Stack() 
    {
        currentStat = craftData.SetMenuType(CraftData.MenuType.STACK);
        Upgrade(); 
    }
    public void Speed()
    {
        currentStat = craftData.SetMenuType(CraftData.MenuType.SPEED);
        Upgrade();
    }

    void Upgrade() { MenuManager.instance.upgrade.UpgradeStat(craftData.GetPrice(currentStat), levelUp); }
    void levelUp() { currentStat.Level++; RenewalInfo(); }
   
}
