using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdMobInterstitial : MonoBehaviour
{
    private InterstitialAd interstitial;

    public void Start()
    {
        //±¤°í ÃÊ±âÈ­
        MobileAds.Initialize(initStatus =>
        {
            RequestInterstitial();
        });
    }
    private void RequestInterstitial()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-2456791029892383/4934651962";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
            string adUnitId = "unexpected_platform";
#endif
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
        this.interstitial.OnAdClosed += HandleOnAdClosed;
    }

    public void AdStart()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
            AdMobManager.instance.AdInit();
        }
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        AdMobManager.instance.AudioPlay();
    }
}
