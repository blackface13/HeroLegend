using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Setting for game (Graphics, audio...)
public class GameSettings : MonoBehaviour {
    public GameObject[] MiscObject; //Set on interface
    public GameObject[] GraphicsObject; //Set on interface
    public GameObject[] AudioObject; //Set on interface
    public GameObject[] LanguageObject; //Set on interface
    public GameObject[] LangObj;
    MapProperties MapBattle;
    string SceneName;
    /// <summary>
    /// Khởi tạo
    /// </summary>
    private void Awake () {
        StartCoroutine (SetLanguage ());
        //Check và gán setting đồ họa
        if (!Module.CheckValue ("SettingGraphics")) //Kiểm tra thiết lập đồ họa đã tồn tại hay chưa
        {
            Module.GameSave ("SettingGraphics", "4"); //Nếu chưa tồn tại, thì set mức đồ họa cao nhất
            GraphicsObject[0].GetComponent<Image> ().enabled = false;
            GraphicsObject[1].GetComponent<Image> ().enabled = false;
            GraphicsObject[2].GetComponent<Image> ().enabled = false;
            GraphicsObject[3].GetComponent<Image> ().enabled = true;
        } else {
            for (int i = 0; i < GraphicsObject.Length; i++) {
                GraphicsObject[i].GetComponent<Image> ().enabled = false;
            }
            for (int i = 0; i < GraphicsObject.Length; i++) {
                GraphicsObject[Convert.ToInt32 (Module.GameLoad ("SettingGraphics")) - 1].GetComponent<Image> ().enabled = true;
                break;
            }
        }
        //Check và gán setting âm nhạc
        //if (!Module.CheckValue ("SettingMusic")) //Kiểm tra thiết lập âm nhạc hay chưa
        //{
        //    Module.GameSave ("SettingMusic", "1"); //Nếu chưa tồn tại, thì bật âm nhạc
        //    GameSystem.Settings.MusicEnable = true;
        //} else
        //    GameSystem.Settings.MusicEnable = Convert.ToBoolean (Convert.ToInt32 (Module.GameLoad ("SettingMusic")));
        ////Check và gán setting âm thanh
        //if (!Module.CheckValue ("SettingSound")) //Kiểm tra thiết lập âm nhạc hay chưa
        //{
        //    Module.GameSave ("SettingSound", "1"); //Nếu chưa tồn tại, thì bật âm nhạc
        //    GameSystem.Settings.SoundEnable = true;
        //} else
        //    GameSystem.Settings.SoundEnable = Convert.ToBoolean (Convert.ToInt32 (Module.GameLoad ("SettingSound")));
        AudioObject[0].GetComponent<Toggle> ().isOn = GameSystem.Settings.MusicEnable;
        AudioObject[1].GetComponent<Toggle> ().isOn = GameSystem.Settings.SoundEnable;
    }

    /// <summary>
    /// Thiết lập ngôn ngữ cho setting
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetLanguage () {
        for (int i = 0; i < 19; i++)
            LangObj[i].GetComponent<Text> ().text = Languages.lang[i + 50];
        LangObj[19].GetComponent<Text> ().text = Languages.lang[62];
        LangObj[20].GetComponent<Text> ().text = Languages.lang[72]; //Thông báo trong feedback
        yield return new WaitForSeconds (0);
    }
    private void Start () {
        SceneName = SceneManager.GetActiveScene ().name;
        if (SceneName.Equals ("Battle")) //Get map nếu đang ở trong battle
            MapBattle = GameObject.FindGameObjectWithTag ("ControlScene").GetComponent<System_Battle> ().Map.GetComponent<MapProperties> ();
        SettingGraphics (Convert.ToInt32 (Module.GameLoad ("SettingGraphics")));
    }

    /// <summary>
    /// Control các button có trong setting
    /// </summary>
    /// <param name="func"></param>
    public void FuncButton (int func) {
        switch (func) {
            case 0: //Exit game
                Application.Quit ();
                break;
            case 1: //OK
                Time.timeScale = 1f;
                Module.PAUSEGAME = false;
                MiscObject[0].SetActive (false); //Đóng bảng setting lại
                break;
            case 2: //Open window Feedback (Mở cửa sổ feedback)
                MiscObject[1].SetActive (true);
                //StartCoroutine(SyncData.GetDomainServer());
                break;
            case 3: //Close window feedback (Đóng cửa sổ feedback)
                MiscObject[1].SetActive (false);
                LangObj[20].GetComponent<Text> ().text = Languages.lang[72]; //Thông báo trong feedback (Đưa về trạng thái cũ)
                //MiscObject[2].GetComponent<InputField>().text = "";
                //MiscObject[3].GetComponent<InputField>().text = "";
                break;
            case 4: //Send feedback (Gửi feedback đi)
                if (MiscObject[4].GetComponent<Text> ().text.Equals (""))
                    LangObj[20].GetComponent<Text> ().text = Languages.lang[70]; //Thông báo trong feedback (chưa nhập tiêu đề)
                else if (MiscObject[5].GetComponent<Text> ().text.Equals (""))
                    LangObj[20].GetComponent<Text> ().text = Languages.lang[71]; //Thông báo trong feedback (chưa nhập nội dung)
                if (!MiscObject[5].GetComponent<Text> ().text.Equals ("") && !MiscObject[4].GetComponent<Text> ().text.Equals ("")) {
                    //StartCoroutine(StartWaitFeedbackSending(0));
                    StartCoroutine (SyncData.Feedback (MiscObject[2].GetComponent<InputField> ().text, MiscObject[3].GetComponent<InputField> ().text, ""));
                    LangObj[20].GetComponent<Text> ().text = Languages.lang[72]; //Thông báo trong feedback (Đưa về trạng thái cũ)
                    MiscObject[1].SetActive (false);
                    MiscObject[2].GetComponent<InputField> ().text = "";
                    MiscObject[3].GetComponent<InputField> ().text = "";
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chờ gửi feedback
    /// </summary>
    /// <param name="_subject"></param>
    /// <param name="_note"></param>
    /// <returns></returns>
    private IEnumerator StartWaitFeedbackSending (float _count) {
        yield return new WaitForSeconds (Module.WAITREQUEST / 10f);
        _count += Module.WAITREQUEST / 10f;
        if (SyncData.stat.Equals (SyncData.State.Waiting))
            StartCoroutine (StartWaitFeedbackSending (_count));
        if (SyncData.stat.Equals (SyncData.State.Success)) //Đã gửi thành công
        {
            MiscObject[1].SetActive (false);
            _count = Module.WAITREQUEST;
            LangObj[20].GetComponent<Text> ().text = Languages.lang[72]; //Thông báo trong feedback (Đưa về trạng thái cũ)
            SyncData.stat = SyncData.State.Waiting;
        }
        if (SyncData.stat.Equals (SyncData.State.LostConnect)) //Chưa gửi dc
        {
            LangObj[20].GetComponent<Text> ().text = Languages.lang[73]; //Thông báo trong feedback (Chưa gửi dc)
            _count = Module.WAITREQUEST;
        }
    }
    /// <summary>
    /// Thiết lập đồ họa, nhạc và âm thanh
    /// </summary>
    /// <param name="level"></param>
    public void SettingGraphics (int level) {
        switch (level) {
            case 1: //Graphics very low
                switch (SceneName) //Change graphics
                {
                    case "Battle":
                        for (int i = 0; i < MapBattle.Detail2.Length; i++)
                            MapBattle.Detail2[i].SetActive (false);
                        for (int i = 0; i < MapBattle.Detail3.Length; i++)
                            MapBattle.Detail3[i].SetActive (false);
                        for (int i = 0; i < MapBattle.Detail4.Length; i++)
                            MapBattle.Detail4[i].SetActive (false);
                        break;
                    default:
                        break;
                }
                GraphicsObject[0].GetComponent<Image> ().enabled = true;
                GraphicsObject[1].GetComponent<Image> ().enabled = false;
                GraphicsObject[2].GetComponent<Image> ().enabled = false;
                GraphicsObject[3].GetComponent<Image> ().enabled = false;
                break;
            case 2: //Graphics low
                switch (SceneName) //Change graphics
                {
                    case "Battle":
                        for (int i = 0; i < MapBattle.Detail2.Length; i++)
                            MapBattle.Detail2[i].SetActive (true);
                        for (int i = 0; i < MapBattle.Detail3.Length; i++)
                            MapBattle.Detail3[i].SetActive (false);
                        for (int i = 0; i < MapBattle.Detail4.Length; i++)
                            MapBattle.Detail4[i].SetActive (false);
                        break;
                    default:
                        break;
                }
                GraphicsObject[0].GetComponent<Image> ().enabled = false;
                GraphicsObject[1].GetComponent<Image> ().enabled = true;
                GraphicsObject[2].GetComponent<Image> ().enabled = false;
                GraphicsObject[3].GetComponent<Image> ().enabled = false;
                break;
            case 3: //Graphics medium
                switch (SceneName) //Change graphics
                {
                    case "Battle":
                        for (int i = 0; i < MapBattle.Detail2.Length; i++)
                            MapBattle.Detail2[i].SetActive (true);
                        for (int i = 0; i < MapBattle.Detail3.Length; i++)
                            MapBattle.Detail3[i].SetActive (true);
                        for (int i = 0; i < MapBattle.Detail4.Length; i++)
                            MapBattle.Detail4[i].SetActive (false);
                        break;
                    default:
                        break;
                }
                GraphicsObject[0].GetComponent<Image> ().enabled = false;
                GraphicsObject[1].GetComponent<Image> ().enabled = false;
                GraphicsObject[2].GetComponent<Image> ().enabled = true;
                GraphicsObject[3].GetComponent<Image> ().enabled = false;
                break;
            case 4: //Graphics hight
                switch (SceneName) //Change graphics
                {
                    case "Battle":
                        for (int i = 0; i < MapBattle.Detail2.Length; i++)
                            MapBattle.Detail2[i].SetActive (true);
                        for (int i = 0; i < MapBattle.Detail3.Length; i++)
                            MapBattle.Detail3[i].SetActive (true);
                        for (int i = 0; i < MapBattle.Detail4.Length; i++)
                            MapBattle.Detail4[i].SetActive (true);
                        break;
                    default:
                        break;
                }
                GraphicsObject[0].GetComponent<Image> ().enabled = false;
                GraphicsObject[1].GetComponent<Image> ().enabled = false;
                GraphicsObject[2].GetComponent<Image> ().enabled = false;
                GraphicsObject[3].GetComponent<Image> ().enabled = true;
                break;
            default:
                break;
        }
        Module.GameSave ("SettingGraphics", level.ToString ()); //Lưu lại thiết lập đồ họa
    }
    public void SettingMusic (bool active) {
        Module.GameSave ("SettingMusic", Convert.ToInt32 (active).ToString ()); //Lưu lại thiết lập âm nhạc
        GameSystem.Settings.MusicEnable = active;
    }
    public void SettingSound (bool active) {
        Module.GameSave ("SettingSound", Convert.ToInt32 (active).ToString ()); //Lưu lại thiết lập âm thanh
        GameSystem.Settings.SoundEnable = active;
    }
}