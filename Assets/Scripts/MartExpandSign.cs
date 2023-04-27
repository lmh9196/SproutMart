using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MartExpandSign : MonoBehaviour
{
    public StageData stageData;
    public Text priceText;
    public Button button;
    public RectTransform priceBackground;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            MenuManager.instance.menuOnOff(MenuManager.instance.upgrade.menu.gameObject);
            MenuManager.instance.UISortDown(MenuManager.instance.upgrade.expandPage.gameObject);
        });
    }
    private void OnEnable()
    {
        priceText.text = GameManager.instance.PriceText(stageData.GetUpgradePrice(), 2);
        GameManager.instance.TextBackgroundSize(priceBackground, priceText);
    }
}
