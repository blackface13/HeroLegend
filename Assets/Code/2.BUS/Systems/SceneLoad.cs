using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoad : MonoBehaviour {
    public Camera cam;
    AsyncOperation asyn = null;
    Text TextLoading;
    Image LoadingBar;
    public void Change_scene (string scn_name) {
        if (GameSystem.ControlActive) {
            try {
                //ADS.HideBanner ();
            } catch { }
            //Nếu vào trận đấu thì check điều kiện đủ 3 nhân vật trong team
            if ((scn_name.Equals ("BattleSystem")) && (DataUserController.Team.Split (';') [0] == "0" || DataUserController.Team.Split (';') [1] == "0" || DataUserController.Team.Split (';') [2] == "0")) {
                GameSystem.ControlFunctions.ShowMessage ((Languages.lang[123]));
            } else {
                var objLoading = Instantiate (Resources.Load<Canvas> ("Prefabs/UI/LoadingSceneCanvas"), new Vector3 (0, 0, -10), Quaternion.identity); //Canvas Loading
                objLoading.GetComponent<Canvas> ().planeDistance = 1;
                TextLoading = objLoading.transform.GetChild (1).GetComponent<Text> ();
                objLoading.GetComponent<Canvas> ().worldCamera = Camera.main;
                LoadingBar = objLoading.transform.GetChild (2).GetComponent<Image> ();
                Time.timeScale = 1f;
                Module.PAUSEGAME = false;
                try {
                    DontDestroyOnLoad (GameSystem.MessageCanvas);
                } catch { }
                //Nếu vào trận đấu
                if (scn_name.Equals ("BattleSystem"))
                    StartCoroutine (GameSystem.FadeOut (GameSystem.BGM, 1f)); //Nhỏ dần nhạc nền rồi tắt
                asyn = SceneManager.LoadSceneAsync (string.IsNullOrEmpty (scn_name) ? Module.PrevScene : scn_name);
            }
        }
    }
    private void Update () {
        if (asyn != null) {
            TextLoading.text = "Loading..." + (asyn.progress * 100).ToString () + "%";
            LoadingBar.rectTransform.localScale = new Vector3 (asyn.progress, 1, 1);
        }
    }
    // private void Awake()
    // {

    // }
    //private void Start()
    //{
    //    Screen.SetResolution(1280, 720, false);
    //    //print(Screen.currentResolution);
    //    //print(Screen.width + ":" + Screen.height);
    //    Change_scene("Battle");
    //}
    public void ChangeCharID (int _charid) {
        Module.GameSave ("CharID", _charid.ToString ());
    }

    // void OnGUI()
    // {
    //     if (asyn != null)
    //     {
    //         GUI.Label(new Rect(cam.pixelWidth / 2, cam.pixelHeight / 2, 200, 50), Convert.ToString(asyn.progress));
    //     }
    // }
}