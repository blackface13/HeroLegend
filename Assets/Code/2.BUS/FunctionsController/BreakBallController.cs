using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakBallController : MonoBehaviour {
    public GameObject[] ObjectController;
    public Text[] TextLanguage;
    public List<GameObject> BallObject; //Các object bóng để đập
    private bool Breaking; //Đang thực hiện animation bóng vỡ (Điều kiện để không cho thao tác các chức năng khác)
    // Start is called before the first frame update
    #region Initialize 

    void Start () {
        if (string.IsNullOrEmpty (API.ServerLink)) //Lấy link server
        {
            StartCoroutine (API.SetupServer ());
            StartCoroutine (WaitingServer (0, 0));
        } else {
            StartCoroutine (API.BreakBall_GetHammerRemaining (DataUserController.User.UserID));
            StartCoroutine (WaitingServer (0, 0));
        }
        // else {
        //    // StartCoroutine (API.BreakBall_GetIntroduction ());//Lấy nội dung hướng dẫn
        //     StartCoroutine (API.BreakBall_GetHammerRemaining (DataUserController.User.UserID));
        //     StartCoroutine (WaitingServer (0, 0));
        // }
        SetupLanguage ();
    }

    /// <summary>
    /// Chờ get thông tin từ server, kết hợp đập búa
    /// </summary>
    /// <param name="type">Chức năng</param>
    /// <returns></returns>
    private IEnumerator WaitingServer (int type, int slot) {
        ObjectController[6].SetActive(true);//Loading
        yield return new WaitUntil (() => API.APIState != API.State.Waiting);
        //Success
        if (API.APIState.Equals (API.State.Success)) {
            switch (type) {
                case 0: //Load búa
                    TextLanguage[1].text = Languages.lang[270] + API.BreakBall_HammerRemaining; //Còn lại
            CreateBallObject ();
                    break;
                case 1: //Thực hiện đập búa
                    //API.BreakBall_HammerRemaining--;
                    TextLanguage[1].text = Languages.lang[270] + API.BreakBall_HammerRemaining; //Còn lại
                    ObjectController[1].SetActive (true);
                    ObjectController[1].transform.position = new Vector3 (BallObject[slot].transform.position.x + 2.2f, BallObject[slot].transform.position.y - 0.3f, 0);
                    ObjectController[1].GetComponent<Animator> ().SetTrigger ("HammerAction");
                    StartCoroutine (GameSystem.HideObject (ObjectController[1], .5f));
                    StartCoroutine (BreakBallEffect (slot));
                    break;
                default:
                    break;
            }
        }
        if (API.APIState.Equals (API.State.Connected)) {
            StartCoroutine (API.BreakBall_GetHammerRemaining (DataUserController.User.UserID));
            CreateBallObject ();
            StartCoroutine (WaitingServer (0, 0));
        }
        if (API.APIState.Equals (API.State.LostConnected)) {
            ObjectController[4].SetActive (true); //Hiển thị nút reconnect
            Breaking = false; //Đưa về trạng thái ko đập búa
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[117])); //"Không kết nối được tới máy chủ";
        }
        if (API.APIState.Equals (API.State.Failed)) {
            Breaking = false; //Đưa về trạng thái ko đập búa
            GameSystem.ControlFunctions.ShowMessage ((Languages.lang[116])); //"Không thực hiện được";
        }
        ObjectController[6].SetActive(false);//Ẩn loading
    }

    /// <summary>
    /// Khởi tạo ngôn ngữ
    /// </summary>
    private void SetupLanguage () {
        TextLanguage[0].text = Languages.lang[269]; //BreakBall
    }

    /// <summary>
    /// Khởi tạo các object bóng để đập
    /// </summary>
    private void CreateBallObject () {
        int xTemp = 0;
        for (int i = 0; i < 10; i++) {
            var temp = i;
            BallObject.Add (Instantiate (Resources.Load<GameObject> ("Prefabs/UI/BreakBallButton"), new Vector3 (0, 0, 0), Quaternion.identity));
            BallObject[i].transform.SetParent (ObjectController[0].transform, false);
            BallObject[i].transform.position = new Vector3 (0 - Camera.main.aspect * (6f - (xTemp * 3)), i > 4 ? -2.5f : 3.64f, 0);
            BallObject[i].GetComponent<Button> ().onClick.AddListener (() => BreakBallAction (temp));
            xTemp++;
            xTemp = ((i + 1) % 5 == 0 && i != 0) ? 0 : xTemp;
        }
    }
    #endregion
    // Update is called once per frame
    // void Update () {

    // }
    #region Functions 

    /// <summary>
    /// Thực hiện đập vỡ bóng
    /// </summary>
    /// <param name="slot"></param>
    private void BreakBallAction (int slot) {
        if (!Breaking) {
            StartCoroutine (API.BreakBall_BreakBallAtion (DataUserController.User.UserID));
            StartCoroutine (WaitingServer (1, slot));
            Breaking = true;
        }
    }

    /// <summary>
    /// Hiệu ứng vỡ bóng
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    private IEnumerator BreakBallEffect (int slot) {
        yield return new WaitForSeconds (0.2f);
        ObjectController[3].transform.position = BallObject[slot].transform.position;
        ObjectController[3].SetActive (true);
        yield return new WaitForSeconds (0.3f);
        ObjectController[2].SetActive (true);
        ObjectController[2].transform.position = BallObject[slot].transform.position;
        ObjectController[3].SetActive (false);
        BallObject[slot].SetActive (false);
        ObjectController[2].GetComponent<Animator> ().SetTrigger ("BallBreak");
        StartCoroutine (GameSystem.HideObject (ObjectController[2], .5f));
        Breaking = false;
    }

    /// <summary>
    /// Button thực hiện các chức năng
    /// </summary>
    public void ButtonFunctions (int type) {
        switch (type) {
            case 0: //Dispose UI
                GameSystem.DisposePrefabUI (0);
                break;
            case 1: //Reconnect
                ObjectController[4].SetActive (false); //Ẩn nút reconnect
                StartCoroutine (API.BreakBall_GetHammerRemaining (DataUserController.User.UserID));
                StartCoroutine (WaitingServer (0, 0));
                break;
            case 2: //Show introduction
                ObjectController[5].SetActive (true); //Hiển thị giao diện introduction
                TextLanguage[2].text = Languages.lang[194]; //Hướng dẫn
                TextLanguage[3].text = Languages.lang[272]; //Nội dung lấy từ server
                break;
            case 3: //Ẩn introduction
                ObjectController[5].SetActive (false); //Ẩn giao diện introduction
                break;
            default:
                break;
        }
    }

    #endregion
}