using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Điều khiển chatbox
public class ChatboxController : MonoBehaviour {
    public GameObject[] Obj;
    private bool FirstOpenChatBox = true; //Check xem có phải lần đầu mở chatbox hay ko, để cuộn xuống cuối cùng
    public Text[] TextObject;
    void Start () {
        TextObject[1].text = Languages.lang[66];
        TextObject[2].text = Languages.lang[122];
        TextObject[3].text = Languages.lang[60];
        Obj[1].GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = Languages.lang[65];
        Obj[3].GetComponent<InputField> ().placeholder.GetComponent<Text> ().text = Languages.lang[122];
        DataUserController.LoadUsername ();
    }
    // Update is called once per frame
    void OnEnable () {
        FirstOpenChatBox = true; //Đưa chatbox về trạng thái mở lần đầu
        DataUserController.LoadUsername ();
        if (string.IsNullOrEmpty (DataUserController.UserName)) //Chưa có tên thì show UI nhập tên
            Obj[2].SetActive (true);
        StartCoroutine (ReloadChatboxLooping ());
    }

    /// <summary>
    /// Button thực hiện chức năng
    /// </summary>
    public void ButtonFunctions (int type) {
        switch (type) {
            case 0: //Close chatbox
                Obj[0].SetActive (false);
                break;
            case 1: //Gửi dòng chat
                StartCoroutine (WaitForPostChatBox (DataUserController.UserName, Obj[1].GetComponent<InputField> ().text));
                Obj[1].GetComponent<InputField> ().text = "";
                break;
            case 2: //Nhập tên
                if (string.IsNullOrEmpty (Obj[3].GetComponent<InputField> ().text))
                    GameSystem.ControlFunctions.ShowMessage( (Languages.lang[121])); //ko ket noi dc may chu
                else {
                    DataUserController.UserName = Obj[3].GetComponent<InputField> ().text;
                    DataUserController.SaveUsername ();
                    Obj[2].SetActive (false);
                }
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// Điều chỉnh lại chiều cao chatbox nếu có sự thay đổi về số dòng
    /// </summary>
    private void RefreshChatboxHeight () {
        //print (TextObject[0].cachedTextGenerator.lineCount);
        if(TextObject[0].cachedTextGenerator.lineCount>1)
        Obj[4].GetComponent<RectTransform> ().sizeDelta = new Vector2 (TextObject[0].GetComponent<RectTransform> ().sizeDelta.x, 57.4f * TextObject[0].cachedTextGenerator.lineCount);
        if (FirstOpenChatBox && TextObject[0].cachedTextGenerator.lineCount>1) { //Nếu là lần đầu mở chatbox => cuộn xuống cuối
            Obj[5].GetComponent<ScrollRect> ().verticalNormalizedPosition = 0f;
            FirstOpenChatBox = false;
        }
        //TextObject[0].GetComponent<RectTransform>().sizeDelta = new Vector2(TextObject[0].GetComponent<RectTransform>().sizeDelta.x, 60*TextObject[0].text.Split ('\n').Length);
    }
    #region Thao tác dữ liệu với server 

    /// <summary>
    /// Refresh dữ liệu từ chatbox về sau khoảng time
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReloadChatboxLooping () {
        if (string.IsNullOrEmpty (API.ServerLink)) {
            GameSystem.ControlFunctions.ShowMessage( (Languages.lang[127])); //Đang thiết lập môi trường
            StartCoroutine (API.SetupServer ());
            yield return new WaitUntil (() => !string.IsNullOrEmpty (API.ServerLink));
            if (!string.IsNullOrEmpty (API.ServerLink)) {
                goto End;
            } else
                GameSystem.ControlFunctions.ShowMessage( (Languages.lang[120])); //ko ket noi dc may chu
        }
        End : yield return new WaitForSeconds (0);
        StartReload:
            StartCoroutine (API.GetContentChatbox ());
        yield return new WaitUntil (() => !string.IsNullOrEmpty (API.ChatboxString));
        if (!string.IsNullOrEmpty (API.ChatboxString) && !API.ChatboxString.Equals ("-1")) {
            TextObject[0].text = API.ChatboxString;
            RefreshChatboxHeight ();
        } else
            GameSystem.ControlFunctions.ShowMessage( (Languages.lang[120])); //ko ket noi dc may chu
        yield return new WaitForSeconds (2);
        goto StartReload;
    }

    /// <summary>
    /// Chờ get dữ liệu chatbox từ server
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForDataChatBox () {
        if (string.IsNullOrEmpty (API.ServerLink)) {
            GameSystem.ControlFunctions.ShowMessage( (Languages.lang[127])); //Đang thiết lập môi trường
            StartCoroutine (API.SetupServer ());
            yield return new WaitUntil (() => !string.IsNullOrEmpty (API.ServerLink));
            if (!string.IsNullOrEmpty (API.ServerLink)) {
                goto End;
            } else
                GameSystem.ControlFunctions.ShowMessage( (Languages.lang[120])); //ko ket noi dc may chu
        }
        End : yield return new WaitForSeconds (0);
        StartCoroutine (API.GetContentChatbox ());
        yield return new WaitUntil (() => !string.IsNullOrEmpty (API.ChatboxString));
        if (!string.IsNullOrEmpty (API.ChatboxString) && !API.ChatboxString.Equals ("-1")) {
            TextObject[0].text = API.ChatboxString;
            RefreshChatboxHeight ();
        } else
            GameSystem.ControlFunctions.ShowMessage( (Languages.lang[120])); //ko ket noi dc may chu
    }

    /// <summary>
    /// Chờ post dữ liệu chatbox lên server
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForPostChatBox (string name, string content) {
        if (!string.IsNullOrEmpty (content)) { //Check xem chatbox có gõ gì ko
            if (string.IsNullOrEmpty (API.ServerLink)) {
                GameSystem.ControlFunctions.ShowMessage( (Languages.lang[127])); //Đang thiết lập môi trường
                StartCoroutine (API.SetupServer ());
                yield return new WaitUntil (() => !string.IsNullOrEmpty (API.ServerLink));
                if (!string.IsNullOrEmpty (API.ServerLink)) {
                    goto End;
                } else
                    GameSystem.ControlFunctions.ShowMessage( (Languages.lang[120])); //ko ket noi dc may chu
            }
            End : yield return new WaitForSeconds (0);
            StartCoroutine (API.PostContentChatbox (name, content));
            yield return new WaitUntil (() => !string.IsNullOrEmpty (API.StatePostChatbox));
            if (!string.IsNullOrEmpty (API.StatePostChatbox) && !API.StatePostChatbox.Equals ("-1")) {
                FirstOpenChatBox = true; //Đưa chatbox về trạng thái mở lần đầu để đẩy xuống cuối
                StartCoroutine (WaitForDataChatBox ());
            } else
                GameSystem.ControlFunctions.ShowMessage( (Languages.lang[120])); //ko ket noi dc may chu
        } else
            GameSystem.ControlFunctions.ShowMessage( (Languages.lang[71])); //Bạn chưa nhập nội dung

    }

    #endregion
}