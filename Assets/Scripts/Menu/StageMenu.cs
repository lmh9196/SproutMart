using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    public StageData stageData;

    public Text priceText;
    public Text levelText;

    public Image moveMaxImage;
    public Button moveBtn;

    public Image maxImage;
    public Button upgradeBtn;

    public bool isMaxCheck;
    bool isSceneCheck;
    
    public void Update()
    {
        isMaxCheck = stageData.Level == stageData.maxLevel ? true : false;
        isSceneCheck = StageManager.instance.saveData.currentSceneName == stageData.senceName ? true : false;


        if (isMaxCheck) { priceText.text = "Max "; }
        else { priceText.text = GameManager.instance.PriceText(stageData.price[stageData.Level], 2); }

        levelText.text = (stageData.Level + 1).ToString();


        upgradeBtn.enabled = !isMaxCheck;
        maxImage.gameObject.SetActive(isMaxCheck);


        moveMaxImage.gameObject.SetActive(isSceneCheck);
        moveBtn.gameObject.SetActive(!isSceneCheck);
    }

    public void Upgrade()
    {
        if (GameManager.instance.SelectGold(GameManager.GoldType.GOLD) >= stageData.price[stageData.Level])
        {
            if (stageData.Level < 5)
            {
                StartCoroutine(StageManager.instance.MartEvent());
                GameManager.instance.CalGold(GameManager.GoldType.GOLD, -stageData.price[stageData.Level]);

                SoundManager.instance.PlaySfx("Buy");
                GameManager.instance.ClickVib();
            }
        }
        else
        {
            SoundManager.instance.PlaySfx("Fail");
            GameManager.instance.ClickVib();
            ColorManager.instance.GoldFail(GameManager.GoldType.GOLD);
        }
    }
    public void Move()
    {
        GameManager.instance.ClickVib();
        SceneManager.LoadScene(stageData.senceName);
        SoundManager.instance.PlaySfx("Pop");
    }
}
