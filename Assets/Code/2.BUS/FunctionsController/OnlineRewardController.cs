using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnlineRewardController : MonoBehaviour {
    public GameObject[] ObjectController;
    public Text[] TextUI;
    private TimeSpan TimeRemaining;
    // print(a.ToString(@"hh\:mm\:ss"));
    void Start () {
        SetupText ();
        FirstSetup ();
    }

    /// <summary>
    /// Khởi tạo ngôn ngữ
    /// </summary>
    private void SetupText () {
        TextUI[1].text = Languages.lang[279]; //Online reward
        TextUI[2].text = Languages.lang[117]; //Can not connect to server
        TextUI[4].text = Languages.lang[280]; //Nhận
    }

    /// <summary>
    /// Đếm ngược thời gian về 0 để nhận thưởng
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowTimeRemaining () {

        ObjectController[0].SetActive (false); //Nút nhận thưởng
        ObjectController[1].SetActive (true); //Kiểu tiền tệ
        ObjectController[2].SetActive (false); //Kiểu item
        ObjectController[3].SetActive (false); //Img loading
        TextUI[0].gameObject.SetActive (true); //Ẩn bộ đếm time
        ObjectController[4].SetActive (false); //Button reconnect

        TextUI[0].gameObject.SetActive (true); //Hiện bộ đếm time
        ObjectController[0].SetActive (false); //Ẩn nút nhận thưởng
        Begin:
            if (GlobalVariables.OnlineRewardTimeRemaining >= 0) {
                //TextUI[0].text = TimeRemaining.ToString (@"hh\:mm\:ss"); //Gán text
                GlobalVariables.OnlineRewardTimeRemaining--;
                TimeRemaining = TimeSpan.FromSeconds (GlobalVariables.OnlineRewardTimeRemaining);
                yield return new WaitForSeconds (1);
                goto Begin;
            } else {
                TextUI[0].gameObject.SetActive (false); //Ẩn bộ đếm time
                ObjectController[0].SetActive (true); //Hiển thị nút nhận thưởng khi hết time
            }
    }

    private void FirstSetup () {
        if (string.IsNullOrEmpty (API.ServerLink)) {
            ObjectController[0].SetActive (false); //Nút nhận thưởng
            ObjectController[1].SetActive (false); //Kiểu tiền tệ
            ObjectController[2].SetActive (false); //Kiểu item
            ObjectController[3].SetActive (false); //Img loading
            TextUI[0].gameObject.SetActive (false); //Ẩn bộ đếm time
            ObjectController[4].SetActive (true); //Button reconnect
            StartCoroutine (WaitingServer (true));
        } else {
            StartCoroutine (API.OnlineRewardAction (DataUserController.User.UserID, 0)); //Lấy thông tin nhận thưởng online
            StartCoroutine (WaitingServer (true));
        }
    }

    /// <summary>
    /// Chờ thông tin kết nối tới server
    /// </summary>
    /// <param name="isRunTime">Có thực hiện chạy thời gian sau khi get hay ko</param>
    /// <returns></returns>
    private IEnumerator WaitingServer (bool isRunTime) {
        ObjectController[3].SetActive (true); //Img loading
        yield return new WaitUntil (() => API.APIState != API.State.Waiting);
        //Success
        if (API.APIState.Equals (API.State.Success)) { //Thực hiện hành động thành công
            TextUI[3].text = GlobalVariables.OnlineRewardMoneyValue.ToString ();
            if (isRunTime)
                StartCoroutine (ShowTimeRemaining ());
            else {
                ObjectController[0].SetActive (false); //Nút nhận thưởng
                ObjectController[1].SetActive (true); //Kiểu tiền tệ
                ObjectController[2].SetActive (false); //Kiểu item
                ObjectController[3].SetActive (false); //Img loading
                TextUI[0].gameObject.SetActive (true); //Ẩn bộ đếm time
                ObjectController[4].SetActive (false); //Button reconnect

                TextUI[0].gameObject.SetActive (true); //Hiện bộ đếm time
                ObjectController[0].SetActive (false); //Ẩn nút nhận thưởng
            }
        }
        if (API.APIState.Equals (API.State.Connected)) { //Kết nối tới server thành công
            StartCoroutine (API.OnlineRewardAction (DataUserController.User.UserID, 0)); //Lấy thông tin nhận thưởng online
            StartCoroutine (WaitingServer (isRunTime));
        }
        if (API.APIState.Equals (API.State.LostConnected)) { //Ko thể kết nối tới server
            ObjectController[0].SetActive (false); //Nút nhận thưởng
            ObjectController[1].SetActive (false); //Kiểu tiền tệ
            ObjectController[2].SetActive (false); //Kiểu item
            ObjectController[3].SetActive (false); //Img loading
            TextUI[0].gameObject.SetActive (false); //Ẩn bộ đếm time
            ObjectController[4].SetActive (true); //Button reconnect
        }
        if (API.APIState.Equals (API.State.Failed)) { //Gửi request nhưng ko thực hiện dc
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[116])); //"Không thực hiện được";
        }
        ObjectController[3].SetActive (false); //Img loading
        ObjectController[5].GetComponent<Home> ().ButtonFunctions (8); //Refresh lại giá trị tiền tệ
    }

    /// <summary>
    /// Thực hiện chức năng
    /// </summary>
    /// <param name="type"></param>
    public void ButtonFunctions (int type) {
        switch (type) {
            case 0: //Reload chức năng
                ObjectController[4].SetActive (false); //Button reconnect
                StartCoroutine (API.OnlineRewardAction (DataUserController.User.UserID, 0)); //Lấy thông tin nhận thưởng online
                StartCoroutine (WaitingServer (true));
                break;
            case 1: //Nhận thưởng
                ObjectController[0].SetActive (false); //Nút nhận thưởng
                StartCoroutine (API.OnlineRewardAction (DataUserController.User.UserID, 1)); //Lấy thông tin nhận thưởng online
                StartCoroutine (WaitingServer (true));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Quay trở lại game từ đa nhiệm
    /// </summary>
    /// <param name="hasFocus"></param>
    void OnApplicationFocus (bool hasFocus) {
        ObjectController[0].SetActive (false); //Nút nhận thưởng
        ObjectController[1].SetActive (false); //Kiểu tiền tệ
        ObjectController[2].SetActive (false); //Kiểu item
        ObjectController[3].SetActive (false); //Img loading
        TextUI[0].gameObject.SetActive (false); //Ẩn bộ đếm time
        ObjectController[4].SetActive (false); //Button reconnect
        StartCoroutine (API.OnlineRewardAction (DataUserController.User.UserID, 0)); //Lấy thông tin nhận thưởng online
        StartCoroutine (WaitingServer (false));
    }

    /// <summary>
    /// Thoát game tạm thời tới
    /// </summary>
    /// <param name="pauseStatus"></param>
    void OnApplicationPause (bool pauseStatus) {

    }
}