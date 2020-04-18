﻿using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
//Quảng cáo admob
public static class ADS {

        public static BannerView bannerView; //Quảng cáo banner
        public static RewardBasedVideoAd rewardBasedVideoSlotInventory; //Quảng cáo tặng thưởng cho slot inventory
        public static RewardBasedVideoAd rewardBasedVideoGems; //Quảng cáo tặng thưởng cho gems
        public static RewardBasedVideoAd rewardBasedVideoGold; //Quảng cáo tặng thưởng cho vàng

        /// <summary>
        /// Khởi tạo
        /// </summary>
        public static void Initialize () {
#if UNITY_ANDROID
                string appId = "ca-app-pub-8267828881809259~1846313456";
#elif UNITY_IPHONE
                string appId = "ca-app-pub-8267828881809259~1846313456";
#else
                string appId = "unexpected_platform";
#endif
                MobileAds.Initialize (appId);

                #region Video mở rộng inventory 

                rewardBasedVideoSlotInventory = RewardBasedVideoAd.Instance;
                rewardBasedVideoSlotInventory.OnAdRewarded += HandleRewardBasedVideoRewardedSlotInventory; //Xem xong sẽ thực hiện tặng thưởng
                rewardBasedVideoSlotInventory.OnAdClosed += HandleRewardBasedVideoClosedSlotInventory;
                RequestRewardBasedVideoSlotInventory ();

                #endregion

                #region Video nhận gems 

                rewardBasedVideoGems = RewardBasedVideoAd.Instance;
                rewardBasedVideoGems.OnAdRewarded += HandleRewardBasedVideoRewardedGems; //Xem xong sẽ thực hiện tặng thưởng
                rewardBasedVideoGems.OnAdClosed += HandleRewardBasedVideoClosedGems;
                RequestRewardBasedVideoGems ();

                #endregion

                #region Video nhận gold 

                rewardBasedVideoGold = RewardBasedVideoAd.Instance;
                rewardBasedVideoGold.OnAdRewarded += HandleRewardBasedVideoRewardedGold; //Xem xong sẽ thực hiện tặng thưởng
                rewardBasedVideoGold.OnAdClosed += HandleRewardBasedVideoClosedGold;
                RequestRewardBasedVideoGold ();

                #endregion
        }

        #region Quảng cáo banner 

        /// <summary>
        /// Khởi chạy quảng cáo banner
        /// </summary>
        /// <param name="type">0: top, 1: bot</param>
        public static void RequestBanner (int type) {
                //MyID: ca-app-pub-8267828881809259/2277558297
                //Test: ca-app-pub-3940256099942544/6300978111
#if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-8267828881809259/2277558297";
#else
                string adUnitId = "unexpected_platform";
#endif

                // Create a 320x50 banner at the top of the screen.
                bannerView = new BannerView (adUnitId, AdSize.Banner, type.Equals (0) ? AdPosition.Top : AdPosition.Bottom);

                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder ().Build ();

                // Load the banner with the request.
                bannerView.LoadAd (request);
        }

        /// <summary>
        /// Ẩn QC banner
        /// </summary>
        public static void HideBanner () {
                bannerView.Hide ();
        }

        #endregion

        #region Video tặng slot inventory 

        /// <summary>
        /// Video slot inventory
        /// </summary>
        public static void RequestRewardBasedVideoSlotInventory () {
                //MyID: ca-app-pub-8267828881809259/2991766365
                //Test: ca-app-pub-3940256099942544/5224354917
                // Get singleton reward based video ad reference.
#if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-8267828881809259/2991766365";
#else
                string adUnitId = "unexpected_platform";
#endif

                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder ().Build ();
                // Load the rewarded video ad with the request.
                rewardBasedVideoSlotInventory.LoadAd (request, adUnitId);
        }

        /// <summary>
        /// Đóng quảng cáo video tặng slot inventory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void HandleRewardBasedVideoClosedSlotInventory (object sender, EventArgs args) {
                RequestRewardBasedVideoSlotInventory ();
        }

        /// <summary>
        /// Tặng thưởng quảng cáo video slot inventory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void HandleRewardBasedVideoRewardedSlotInventory (object sender, Reward args) {
                string type = args.Type;
                var amount = (float) args.Amount;
                DataUserController.User.InventorySlot += amount; //Cộng thêm slot cho user
                DataUserController.SaveUserInfor ();
                RequestRewardBasedVideoSlotInventory ();
                //Load lại inventory
                var scload = new SceneLoad ();
                scload.Change_scene ("Inventory");
        }

        #endregion

        #region Video tặng Gems 

        /// <summary>
        /// Video slot inventory
        /// </summary>
        public static void RequestRewardBasedVideoGems () {
                //MyID: ca-app-pub-8267828881809259/9189129152
                //Test: ca-app-pub-3940256099942544/5224354917
                // Get singleton reward based video ad reference.
#if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
                string adUnitId = "unexpected_platform";
#endif

                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder ().Build ();
                // Load the rewarded video ad with the request.
                rewardBasedVideoGems.LoadAd (request, adUnitId);
        }

        /// <summary>
        /// Đóng quảng cáo video tặng gems
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void HandleRewardBasedVideoClosedGems (object sender, EventArgs args) {
                RequestRewardBasedVideoGems ();
        }

        /// <summary>
        /// Tặng thưởng quảng cáo video gems
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void HandleRewardBasedVideoRewardedGems (object sender, Reward args) {
                string type = args.Type;
                var amount = (float) args.Amount;
                DataUserController.User.Gems += amount; //Cộng thêm gem cho user
                DataUserController.SaveUserInfor ();
                RequestRewardBasedVideoGems ();
        }

        #endregion

        #region Video tặng Gold 

        /// <summary>
        /// Video Gold
        /// </summary>
        public static void RequestRewardBasedVideoGold () {
                //MyID: ca-app-pub-8267828881809259/8933978281
                //Test: ca-app-pub-3940256099942544/5224354917
                // Get singleton reward based video ad reference.
#if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
                string adUnitId = "unexpected_platform";
#endif

                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder ().Build ();
                // Load the rewarded video ad with the request.
                rewardBasedVideoGold.LoadAd (request, adUnitId);
        }

        /// <summary>
        /// Đóng quảng cáo video tặng Gold
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void HandleRewardBasedVideoClosedGold (object sender, EventArgs args) {
                RequestRewardBasedVideoGold ();
        }

        /// <summary>
        /// Tặng thưởng quảng cáo video Gold
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void HandleRewardBasedVideoRewardedGold (object sender, Reward args) {
                string type = args.Type;
                var amount = (float) args.Amount;
                DataUserController.User.Golds += amount; //Cộng thêm gem cho user
                DataUserController.SaveUserInfor ();
                RequestRewardBasedVideoGold ();
        }

        #endregion

}