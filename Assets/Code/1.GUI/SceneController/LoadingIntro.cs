using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
//using StartApp;
//Loading đầu game
public class LoadingIntro : MonoBehaviour {
    private string SavePolicy = "AcceptPolicy";
    SceneLoad ScnLoad = new SceneLoad ();
    public GameObject[] Obj;
    // Use this for initialization
    void Start ()
    {
        //AD StarApp
 //       AdSdk.Instance.SetUserConsent(
 //"pas",
 //true,
 //(long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);

 //       AdSdk.Instance.ShowSplash();

        #region Khởi tạo hoặc set Canvas thông báo cho Scene 
        try {
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
        } catch {
            GameSystem.Initialize (); //Khởi tạo này dành cho scene nào test ngay
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
            GameSystem.MessageCanvas.GetComponent<Canvas> ().planeDistance = 1;
        }
        #endregion

        GameSystem.ControlFunctions.SetupServer();//Khởi tạo server khi mở game
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ErrorCode.Initialize();//Khởi tạo các mã lỗi
        StartCoroutine (WaitForShowPolicy ());
        MobileAds.Initialize(initStatus => { });
        //ADS.Initialize();
        //ADS.RequestBanner(0);
        ItemDropController.Initialize ();
    }
    private IEnumerator WaitForShowPolicy () {
        yield return new WaitUntil (() => !Obj[0].activeSelf); //Chờ logo xuất hiện xong

        if (string.IsNullOrEmpty (PlayerPrefs.GetString (SavePolicy))) //Nếu chưa đồng ý với điều khoản hoặc chơi game lần đầu
        {
            Obj[1].SetActive (true); //Show policy
        } else {
            DataUserController.LoadAll ();
            ScnLoad.Change_scene ("Home");
        }
    }
    /// <summary>
    /// Chấp nhận điều khoản
    /// </summary>
    public void ButtonTickAcceptPolicy () {
        Obj[2].SetActive (!Obj[2].activeSelf);
        Obj[3].SetActive (Obj[2].activeSelf);
    }

    /// <summary>
    /// Không chấp nhập điều khoản, out game
    /// </summary>
    public void ButtonCancel () {
        Application.Quit ();
    }

    public void Accept () {
        DataUserController.LoadAll ();
        PlayerPrefs.SetString (SavePolicy, "True");
        ScnLoad.Change_scene ("Home");
    }
}