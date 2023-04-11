using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatueBtn : MonoBehaviour
{
    public Statue statue;
    public Text priceText;
    public Button buyBtn;
    public Image profileImage;
    RectTransform profileImageRect;
    public Image coinImage;

    private void Awake()
    {
        profileImageRect = profileImage.GetComponent<RectTransform>();
    }

    void Start() 
    { 
        buyBtn.onClick.AddListener(() => MenuManager.instance.statues.BuyStatue(statue));
        profileImage.sprite = statue.childRenderer.sprite;
        profileImage.SetNativeSize();
        InitProfileImage();
        switch (statue.goldType)
        {
            case GameManager.GoldType.GOLD: coinImage.sprite = GameManager.instance.goldSprite; break;
            case GameManager.GoldType.GEM: coinImage.sprite = GameManager.instance.gemSprite; break;
        }
    }
    void Update() { priceText.text = GameManager.instance.PriceText(statue.data.price, 2); }


    void InitProfileImage()
    {
        profileImage.sprite = statue.childRenderer.sprite;
        profileImage.SetNativeSize();

        CalSize();
    }


    void CalSize()
    {
        float tempMinusXsize = 0;
        float tempMinusYsize = 0;

        if (profileImageRect.sizeDelta.x > 240) { tempMinusXsize = profileImageRect.sizeDelta.x - 240; }
        if (profileImageRect.sizeDelta.y > 240) { tempMinusYsize = profileImageRect.sizeDelta.y - 240; }


        float reseultMinusSize = tempMinusXsize > tempMinusYsize ? tempMinusXsize : tempMinusYsize;


        profileImageRect.sizeDelta = new Vector2(profileImageRect.sizeDelta.x- reseultMinusSize, profileImageRect.sizeDelta.y - reseultMinusSize);
    }
}
