using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
//Quảng cáo admob
public static class ADS
{

    public static BannerView bannerView; //Quảng cáo banner
    public static RewardBasedVideoAd rewardBasedVideoGems; //Quảng cáo tặng thưởng cho gems

    /// <summary>
    /// Khởi tạo
    /// </summary>
    public static void Initialize()
    {
#if UNITY_ANDROID
        string appId = "ca-app-pub-1566576781523718~1831231195";
#elif UNITY_IPHONE
                string appId = "ca-app-pub-8267828881809259~1846313456";
#else
                string appId = "unexpected_platform";
#endif
        MobileAds.Initialize(appId);

        #region Video nhận gems 

        rewardBasedVideoGems = RewardBasedVideoAd.Instance;
        rewardBasedVideoGems.OnAdRewarded += HandleRewardBasedVideoRewardedGems; //Xem xong sẽ thực hiện tặng thưởng
        rewardBasedVideoGems.OnAdClosed += HandleRewardBasedVideoClosedGems;
        RequestRewardBasedVideoGems();

        #endregion
    }

    #region Quảng cáo banner 

    /// <summary>
    /// Khởi chạy quảng cáo banner
    /// </summary>
    /// <param name="type">0: top, 1: bot</param>
    public static void RequestBanner(int type)
    {
        //MyID: ca-app-pub-1566576781523718/6076300799
        //Test: ca-app-pub-3940256099942544/6300978111
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1566576781523718/6076300799";
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-1566576781523718/6076300799";
#else
                string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, type.Equals(0) ? AdPosition.Top : AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    /// <summary>
    /// Ẩn QC banner
    /// </summary>
    public static void HideBanner()
    {
        try
        {
            bannerView.Hide();
        }
        catch { }
    }

    public static void ShowBanner()
    {
        try
        {
            bannerView.Show();
        }
        catch { }
    }

    #endregion

    #region Quảng cáo xen kẽ
    public static InterstitialAd interstitial;

    public static void RequestInterstitial()
    {
        //MyID: ca-app-pub-1566576781523718/1370769028
        //Test: ca-app-pub-3940256099942544/1033173712
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-1566576781523718/1370769028";
#else
        string adUnitId = "unexpected_platform";
#endif
        //interstitial = null;


        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

    }

    public static void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdLoaded event received");
    }

    public static void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        //MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public static void HandleOnAdOpened(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdOpened event received");
    }

    public static void HandleOnAdClosed(object sender, EventArgs args)
    {
        //MonoBehaviour.print("HandleAdClosed event received");
    }

    public static void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        // MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    #endregion

    #region Video tặng Gems 

    /// <summary>
    /// Video slot inventory
    /// </summary>
    public static void RequestRewardBasedVideoGems()
    {
        //MyID: ca-app-pub-1566576781523718/8866363080
        //Test: ca-app-pub-3940256099942544/5224354917
        // Get singleton reward based video ad reference.
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1566576781523718/8866363080";
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
                string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        rewardBasedVideoGems.LoadAd(request, adUnitId);
    }

    /// <summary>
    /// Đóng quảng cáo video tặng gems
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    static void HandleRewardBasedVideoClosedGems(object sender, EventArgs args)
    {
        RequestRewardBasedVideoGems();
    }

    /// <summary>
    /// Tặng thưởng quảng cáo video gems
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public static void HandleRewardBasedVideoRewardedGems(object sender, Reward args)
    {
        //string type = args.Type;
        var amount = (float)args.Amount;
        DataUserController.User.Gems += amount; //Cộng thêm gem cho user
        DataUserController.SaveUserInfor();
        RequestRewardBasedVideoGems();
    }

    #endregion

}