using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.EventSystems;

public class AdMobReward : MonoBehaviour
{
    string adUnitId;


    private RewardedAd rewardedAd;
    AdMobManager adMobManager;
    private void Awake()
    {
        adMobManager = GetComponent<AdMobManager>();
    }

    public void Start() 
    {
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
		adUnitId = "ca-app-pub-6754544778509872/7165886378"; 
#else
		adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void Show() 
    {
        if (GameManager.instance.checkList.IsBuyRemoveAD)
        {
            adMobManager.adMobRewardAct?.Invoke();
            AdMobManager.instance.AudioPlay();
            SoundManager.instance.PlaySfx("RightChoice");
        }
        else
        {
            StartCoroutine(ShowRewardAd());
            AdMobManager.instance.AdInit();
            AdMobManager.instance.popUpContents.askRemoveAD.SetActive(true);
        }

        for (int i = 0; i < adMobManager.adStruct.Length; i++)
        {
            if (adMobManager.adStruct[i].button.gameObject == EventSystem.current.currentSelectedGameObject)
            {
                adMobManager.adStruct[i].button.gameObject.SetActive(false);
                adMobManager.adStruct[i].defaultSpawnTime = adMobManager.TimerReset(adMobManager.adStruct[i]);
                break;
            }
        }
    }

    IEnumerator ShowRewardAd() 
    {
        while (!rewardedAd.IsLoaded()) { yield return null; }

        rewardedAd.Show();
    }

    public void ReloadAd() 
    {
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-2456791029892383/3649490453";
#elif UNITY_IOS
            adUnitId = "ca-app-pub-6754544778509872/7165886378";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }


    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        ReloadAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        adMobManager.adMobRewardAct?.Invoke();
        AdMobManager.instance.AudioPlay();
        SoundManager.instance.PlaySfx("RightChoice");
    }
}
