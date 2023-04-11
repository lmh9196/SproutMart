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

        if (lockCover != null && craftData.MenuLockCoverAct == null)
        {
            craftData.MenuLockCoverAct = () => { lockCover.SetActive(!craftData.IsAchiveTrigger); };
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

            if (craftData.stat[i].Level < craftData.stat[i].needPrice.Length)
            {
                menu[i].priceText.text = GameManager.instance.PriceText(craftData.SetPrice(craftData.stat[i]), 2);
            }
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

    void Upgrade() { MenuManager.instance.upgrade.UpgradeStat(craftData.SetPrice(currentStat), levelUp); }
    void levelUp() { currentStat.Level++; RenewalInfo(); }
   
}
