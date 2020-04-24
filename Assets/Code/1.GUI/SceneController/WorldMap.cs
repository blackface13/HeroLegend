using System;
using System.Collections;
using System.Collections.Generic;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorldMap : MonoBehaviour {
    public GameObject[] Objects;
    public Text[] TextLanguage;
    // Start is called before the first frame update
    void Start () {
        //Languages.SetupLanguage(0);
        Module.PrevScene = "Room";
        ADS.ShowBanner();
        #region Khởi tạo hoặc set Canvas thông báo cho Scene 

        try {
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
        } catch {
            GameSystem.Initialize (); //Khởi tạo này dành cho scene nào test ngay
            GameSystem.MessageCanvas.GetComponent<Canvas> ().worldCamera = Camera.main;
            GameSystem.MessageCanvas.GetComponent<Canvas> ().planeDistance = 1;
        }
        #endregion
        #region Khởi tạo các infor box 

        //Tạo tọa độ để hiển thị infor box thuê sát thủ
        // var pos = Objects[12].transform.position - new Vector3 (Camera.main.ScreenToWorldPoint (Objects[12].GetComponent<RectTransform> ().sizeDelta).x / 2, Camera.main.ScreenToWorldPoint (Objects[12].GetComponent<RectTransform> ().sizeDelta).y / 2, 0);
        // var rect = new Vector2(1000, 200);
        // GameSystem.CreateBoxDownUp (Objects[12], (e) => GameSystem.ShowBoxInformation (rect, pos,Objects[12].GetComponent<RectTransform> ().sizeDelta, 1, "fdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdg\nfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfgfdgfdsggsdfg"), (e) => GameSystem.HideBoxInformation ());

        #endregion
        //Mặc định lựa chọn chế độ preview khi open scene
        Module.BattleModeSelected = 0;
        Objects[12].transform.position = Objects[13].transform.position;
        TextLanguage[14].text = Languages.lang[255]; //Text hướng dẫn
        SetupTextLanguage ();
    }

    /// <summary>
    /// Khởi tạo ngôn ngữ trong scene
    /// </summary>
    private void SetupTextLanguage () {
        for (int i = 0; i < 7; i++) //Tên vùng đất
            TextLanguage[i].text = Languages.lang[168 + i];
        TextLanguage[8].text = Languages.lang[197]; //Xác nhận
        TextLanguage[9].text = Languages.lang[67]; //Hủy bỏ
        TextLanguage[10].text = Languages.lang[198]; //Thuê sát thủ
        TextLanguage[11].text = Languages.lang[252]; //preview
        TextLanguage[12].text = Languages.lang[253]; //random
        TextLanguage[13].text = Languages.lang[254]; //survival
    }

    /// <summary>
    /// Lựa chọn vùng bản đồ
    /// </summary>
    /// <param name="regionID">0: rừng rậm</param>
    public void ButtonSelectRegion (int regionID) {
        TextLanguage[7].text = TextLanguage[regionID].text; //Set tên vùng lựa chọn
        //if (!regionID.Equals (4)) {
            Objects[10].SetActive (false); //Ẩn hết các object hiệu ứng
            Objects[9].SetActive (true); //Open form chọn map
            Module.WorldMapRegionSelected = (sbyte) regionID;
            //Nếu chưa set đội hình team địch thì set
            if (string.IsNullOrEmpty (DataUserController.User.EnemyFutureMap[regionID])) {
                DataUserController.User.EnemyFutureMap[regionID] =
                    BattleCore.HeroIDLine3[UnityEngine.Random.Range (0, BattleCore.HeroIDLine3.Count)].ToString () + ";" +
                    BattleCore.HeroIDLine2[UnityEngine.Random.Range (0, BattleCore.HeroIDLine2.Count)].ToString () + ";" +
                    BattleCore.HeroIDLine1[UnityEngine.Random.Range (0, BattleCore.HeroIDLine1.Count)].ToString ();
                DataUserController.SaveUserInfor ();
            }
        //} else GameSystem.ControlFunctions.ShowMessage( ("Comming Soon...!"));
    }

    /// <summary>
    /// Chức năng các button trong scene
    /// </summary>
    /// <param name="type">chức năng của button</param>
    public void ButtonFunctions (int type) {
        switch (type) {
            case 0: //Đóng form chọn map
                Objects[10].SetActive (true); //Hiển thị các object hiệu ứng
                Objects[9].SetActive (false); //Close form chọn map
                break;
            case 1: //Vào scene room
                var sceneLoad = new SceneLoad ();
                sceneLoad.Change_scene ("Room");
                break;
            case 2: //Chức năng thuê sát thủ
                //Show dialog xác nhận bán item
                GameSystem.ShowConfirmDialog (string.Format (Languages.lang[199], Module.GemsForHireAssassin));
                //Chờ lệnh confirm
                StartCoroutine (ActionHireAssassin ());
                break;
            case 3: //Chọn mode preview
                Module.BattleModeSelected = 0;
                Objects[12].transform.position = Objects[13].transform.position; //Vị trí hiệu ứng lựa chọn
                TextLanguage[14].text = Languages.lang[255]; //Text hướng dẫn
                break;
            case 4: //Chọn mode random
                Module.BattleModeSelected = 1;
                Objects[12].transform.position = Objects[14].transform.position; //Vị trí hiệu ứng lựa chọn
                TextLanguage[14].text = Languages.lang[256]; //Text hướng dẫn
                break;
            case 5: //Chọn mode survival
                GameSystem.ControlFunctions.ShowMessage( (Languages.lang[2])); //Comming soon
                // Module.BattleModeSelected = 2;
                // Objects[12].transform.position = Objects[15].transform.position; //Vị trí hiệu ứng lựa chọn
                // TextLanguage[14].text = Languages.lang[257]; //Text hướng dẫn
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Chờ xác nhận thuê sát thủ
    /// </summary>
    private IEnumerator ActionHireAssassin () {
        yield return new WaitUntil (() => GameSystem.ConfirmBoxResult != 0);
        //Accept
        if (GameSystem.ConfirmBoxResult == 1) {
            if(UserSystem.CheckGems((float)Module.GemsForHireAssassin))
            {
                UserSystem.DecreaseGems((float)Module.GemsForHireAssassin);
                BattleCore.ChangeDifficult(1, Module.WorldMapRegionSelected); //Giảm độ khó của map hiện tại
                DataUserController.SaveUserInfor(); //Lưu thông tin user
                GameSystem.ControlFunctions.ShowMessage((Languages.lang[251])); //Thông báo thuê sát thủ thành công
            }
        }
    }
}