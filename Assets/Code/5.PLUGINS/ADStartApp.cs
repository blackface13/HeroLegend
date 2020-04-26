using UnityEngine;
using System.Collections;
using StartApp;
using System;

public static class ADStartApp
{

	// Use this for initialization
	public static void ShowBanner()
	{
		AdSdk.Instance.SetUserConsent(
 "pas",
 true,
 (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
		AdSdk.Instance.ShowDefaultBanner(BannerAd.BannerPosition.Top);
	}
}
