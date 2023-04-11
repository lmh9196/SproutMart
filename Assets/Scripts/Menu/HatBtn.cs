using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HatBtn : MonoBehaviour
{
    public HatData hatData;

    [SerializeField] Button activeButton;

    GameObject priceCover;
    GameObject buyCover;
    Text priceText;
    public Image coinImage;
    public Image goodsImage;

    private void Awake()
    {
        priceCover = transform.Find("Price").gameObject;
        priceText = priceCover.transform.Find("PriceText").GetComponent<Text>();
        buyCover = transform.Find("BuyCover").gameObject;
    }
    private void Start()
    {
        MenuManager.instance.hats.AddHatActiveButton(activeButton, hatData);
        coinImage.sprite = MenuManager.instance.hats.InitCoinImage(hatData.goldType);
        goodsImage.sprite = hatData.goodsSprite;
        goodsImage.SetNativeSize();
    }

    void Update()
    {
        priceText.text = GameManager.instance.PriceText(hatData.HatPrice(),2);

        if (hatData.isBought)
        {
            priceCover.SetActive(false);
            buyCover.SetActive(true);
        }
        else
        {
            priceCover.SetActive(true);
            buyCover.SetActive(false);
        }
    }
}
