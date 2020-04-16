/* Created: Bkack Face (bf.blackface@gmail.com)
 * Setting for game
 * 2018/12/01
 */

using System;
using System.Collections;
using System.Collections.Generic;
//using Facebook.MiniJSON;
//using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    public GameObject[] Obj;
    public GameObject[] ObjectController;
    public Text[] TextUI;
    public Text[] TextSetLanguage;
    // Use this for initialization
    private void Start () {
        SetText ();
        LoadInfor();
    }
    /// <summary>
    /// Set text ngôn ngữ cho object
    /// </summary>
    private void SetText () {

        TextUI[0].text = Languages.lang[57];//Âm nhạc
        TextUI[1].text = Languages.lang[58];//Âm thanh
        TextUI[2].text = Languages.lang[124];//Tiết kiệm pin
        TextUI[3].text = Languages.lang[162];// = "Cài đặt hệ thống";
        TextUI[4].text = Languages.lang[163];// = "Cài đặt trận đấu";
        TextUI[5].text = Languages.lang[59];// = "Ngôn ngữ";
        TextUI[6].text = Languages.lang[161];// = "Nút bấm kỹ năng bên phải";
        TextUI[7].text = Languages.lang[164];// = "slow motion";
        TextUI[8].text = Languages.lang[50];
        TextUI[9].text = Languages.lang[62];
        TextUI[10].text = Languages.lang[63];
        TextUI[11].text = Languages.lang[66];


        //TextSetLanguage[0].text = Languages.lang[50];
        //TextSetLanguage[1].text = Languages.lang[57] + (GameSystem.Settings.MusicEnable ? Languages.lang[125] : Languages.lang[126]);
        //TextSetLanguage[2].text = Languages.lang[58] + (GameSystem.Settings.SoundEnable?Languages.lang[125] : Languages.lang[126]);
        //TextSetLanguage[3].text = Languages.lang[59];
        //TextSetLanguage[4].text = Languages.lang[62];
        //TextSetLanguage[5].text = Languages.lang[63];
        //TextSetLanguage[6].text = Languages.lang[66];
        //TextSetLanguage[7].text = Languages.lang[124] + (GameSystem.Settings.FPSLimit.Equals (60) ? Languages.lang[126] : Languages.lang[125]);
        //TextSetLanguage[8].text = Languages.lang[161] + (!GameSystem.Settings.ButtonBattle ? Languages.lang[162] : Languages.lang[163]);
        //TextSetLanguage[9].text = Languages.lang[164] + (!GameSystem.Settings.SkillSlowMotion? Languages.lang[126] : Languages.lang[125]);


        Obj[2].GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = Languages.lang[64];
        Obj[3].GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = Languages.lang[65];
        Obj[4].GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = Languages.lang[68];
    }

    /// <summary>
    /// Load dữ liệu 
    /// </summary>
    private void LoadInfor()
    {
        ObjectController[0].SetActive(GameSystem.Settings.MusicEnable);//Music
        ObjectController[1].SetActive(GameSystem.Settings.SoundEnable);//Sound
        ObjectController[2].SetActive(GameSystem.Settings.FPSLimit.Equals(60) ? false:true);//SaveBattery
        ObjectController[3].SetActive(GameSystem.Settings.ButtonBattle);//Skill button
        ObjectController[4].SetActive(GameSystem.Settings.SkillSlowMotion);//SaveBattery
    }    

    /// <summary>
    /// Mở giao diện chọn ngôn ngữ
    /// </summary>
    public void OpenUISelectLanguage () {
        Obj[1].SetActive (true);
    }

    /// <summary>
    /// Chọn ngôn ngữ
    /// 0: English
    /// 1: Vietnamese
    /// </summary>
    public void ButtonSelectLanguage (int type) {
        switch (type) {
            case 0:
                Languages.SetupLanguage (0);
                break;
            case 1:
                Languages.SetupLanguage (1);
                break;
            default:
                break;
        }
        SetText();
        DataUserController.SaveData(DataUserController.StrSaveSetting, JsonUtility.ToJson(GameSystem.Settings));
        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[103]));
        Obj[1].SetActive (false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());

    }

    /// <summary>
    /// Mở giao diện chọn ngôn ngữ
    /// -1: out game
    /// 0: Music
    /// 1: sound
    /// 2: language
    /// 3: feedback
    /// 4: set FPS
    /// 5: đổi button trái phải trong combat
    /// 6: cho phép slow motion của skill trong combat
    /// 7: Login facebook
    /// </summary>
    public void ButtonSettingController (int type) {
        switch (type) {
            case -2:
                GameSystem.ShowConfirmDialog(Languages.lang[102]);
                StartCoroutine(WaitForConfirmExitGame());
                break;
            case -1:
                GameSystem.DisposePrefabUI(11);
                break;
            case 0: //Music
                GameSystem.Settings.MusicEnable = !GameSystem.Settings.MusicEnable;
                if (GameSystem.Settings.MusicEnable)
                    GameSystem.RunBGM (0);
                else
                    GameSystem.StopBGM ();
                break;
            case 1: //Sound
                GameSystem.Settings.SoundEnable = !GameSystem.Settings.SoundEnable;
                break;
            case 2:
                Obj[1].SetActive (true);
                break;
            case 3:
                Obj[5].SetActive (true);
                break;
            case 4: //Khung hình
                GameSystem.Settings.FPSLimit = GameSystem.Settings.FPSLimit.Equals (60) ? 30 : 60;
                Application.targetFrameRate = GameSystem.Settings.FPSLimit;
                break;
            case 5: //Vị trí nút bấm skill trong battle
                GameSystem.Settings.ButtonBattle = !GameSystem.Settings.ButtonBattle;
                break;
            case 6: //Setting skill slow motion
                GameSystem.Settings.SkillSlowMotion = !GameSystem.Settings.SkillSlowMotion;
                break;
            case 7:
                //Button_login_FB ();
                break;
            default:
                GameSystem.ControlFunctions.ShowMessage (("Comming Soon..."));
                break;
        }
        DataUserController.SaveData(DataUserController.StrSaveSetting, JsonUtility.ToJson(GameSystem.Settings));
        LoadInfor();
    }

    /// <summary>
    /// Button gửi phản hồi
    /// </summary>
    public void ButtonSendFeedback () {
        if (string.IsNullOrEmpty (Obj[2].GetComponent<InputField> ().text)) {
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[70]));
            return;
        }
        if (string.IsNullOrEmpty (Obj[3].GetComponent<InputField> ().text)) {
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[71]));
            return;
        }
        StartCoroutine (SyncData.Feedback (Obj[2].GetComponent<InputField> ().text, Obj[3].GetComponent<InputField> ().text, Obj[4].GetComponent<InputField> ().text));
        GameSystem.ControlFunctions.ShowMessage ((Languages.lang[72]));
        Obj[2].GetComponent<InputField> ().text = "";
        Obj[3].GetComponent<InputField> ().text = "";
        Obj[4].GetComponent<InputField> ().text = "";
        HideUI (2);

    }

    /// <summary>
    /// Chờ xác nhận thoát game
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator WaitForConfirmExitGame () {
        yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);
        if (GameSystem.ConfirmBoxResult == 1)
            Application.Quit ();
    }

    /// <summary>
    /// Ẩn giao diện
    /// </summary>
    /// <param name="type">0: giao diện setting, 1: giao diện chọn ngôn ngữ, 2: giao diện feecback</param>
    public void HideUI (int type) {
        switch (type) {
            case 0:
                Obj[0].SetActive (false);
                break;
            case 1:
                Obj[1].SetActive (false);
                break;
            case 2:
                Obj[5].SetActive (false);
                break;
            default:
                break;
        }
    }
    // #region FB login
    // public void Button_login_FB () {
    //     if (!FB.IsInitialized) {
    //         // Initialize the Facebook SDK
    //         FB.Init (InitCallback, OnHideUnity);
    //     } else {
    //         // Already initialized, signal an app activation App Event
    //         FB.ActivateApp ();
    //     }
    // }
    // private void AuthCallback (ILoginResult result) {
    //     if (FB.IsLoggedIn) {
    //         // AccessToken class will have session details
    //         var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
    //         // Print current access token's User ID
    //         //Debug.Log(aToken.UserId);
    //         var userID = aToken.UserId;
    //         // Print current access token's granted permissions
    //         foreach (string perm in aToken.Permissions) {
    //             //Debug.Log(perm);
    //         }
    //     } else {
    //         //Debug.Log("User cancelled login");
    //     }
    // }
    // void GetFacebookData (IResult result) {
    //     //string fbName = result.ResultDictionary["name"].ToString();
    //     string fbName = result.ResultDictionary["name"].ToString ();
    //     TextSetLanguage[10].text = fbName;
    //     // print (fbName);
    //     // Debug.Log ("fbName: " + result);
    // }
    // private void InitCallback () {
    //     if (FB.IsInitialized) {
    //         // Signal an app activation App Event
    //         FB.ActivateApp ();
    //         // Continue with Facebook SDK
    //         // ...

    //         List<string> perms = new List<string> () { "public_profile", "email", "user_friends" };
    //         FB.LogInWithReadPermissions (perms, AuthCallback);
    //         FB.API ("me?fields=name", Facebook.Unity.HttpMethod.GET, GetFacebookData);
    //     } else {
    //         // bol[2] = false;
    //         //Debug.Log("Failed to Initialize the Facebook SDK");
    //     }
    // }
    // private void OnHideUnity (bool isGameShown) {
    //     if (!isGameShown) {
    //         // Pause the game - we will need to hide
    //         Time.timeScale = 0;
    //     } else {
    //         // Resume the game - we're getting focus again
    //         Time.timeScale = 1;
    //     }
    // }
    // #endregion
}