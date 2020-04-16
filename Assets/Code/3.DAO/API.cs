/* Created: Bkack Face (bf.blackface@gmail.com)
 * API
 */
using Assets.Code._4.CORE;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

using Assets.Code._0.DTO.UserInforModels;
using Assets.Code._0.DTO.Models;

public static class API
{

    #region Khởi tạo variables 
    private static string SecurityKey = "";
    public static string Server = "https://xn--mten-1ua4066b.vn/CreateServer.php";
    public static string ServerLink = "";
    private static string ChatboxGetLink = "";
    private static string ChatboxPostLink = "";
    public static string ChatboxString = "";
    public static string StatePostChatbox = ""; //Trạng thái post chatbox dc hay chưa
    public static State APIState = State.LostConnected;
    public enum State
    {
        Waiting, //Trạng thái chờ kết nối server
        Success, //Thao tác thành công với server
        Failed, //Thao tác thất bại, hoặc server từ chối yêu cầu
        Connected, //Kết nối tới server thành công
        LostConnected, //Không thể kết nối tới server
        SyncFailedByHWID,//Không thể đồng bộ do đăng ký chơi trên thiết bị này nhưng đồng bộ trên thiết bị khác
    }
    public static int BreakBall_HammerRemaining = -1; //Số búa còn lại
    #endregion

    public static IEnumerator SetupServer()
    {
        //if (string.IsNullOrEmpty(ServerLink))
        //{
        //    var www = UnityWebRequest.Get(Server);
        //    www.SetRequestHeader("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
        //    yield return www.SendWebRequest();
        //    string result = www.downloadHandler.text;
        //    if (www.isNetworkError || www.isHttpError)
        //    {
        //        APIState = State.LostConnected;
        //    }
        //    else
        //    {
        //        var resultArray = result.Split('\n');
        //        ChatboxGetLink = resultArray[0];
        //        ChatboxPostLink = resultArray[1];
        //        ServerLink = resultArray[2];
        //        // Debug.Log(ChatboxGetLink);
        //        // Debug.Log(ChatboxPostLink);
        //        // Debug.Log(ServerLink);
        //        APIState = State.Connected;
        //    }
        //}
        yield return null;
    }

    #region API Break Ball 

    /// <summary>
    /// Lấy thông tin hướng dẫn đập bóng
    /// </summary>
    /// <returns></returns>
    public static IEnumerator BreakBall_GetIntroduction()
    {
        //APIState = State.Waiting;
        var www = UnityWebRequest.Get(ServerLink + "BreakBallIntroduction.php?lang=" + (Module.SettingLanguage.Equals(1) ? "vi" : "en"));
        www.SetRequestHeader("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        if (www.isNetworkError || www.isHttpError) { }
        else
        {
            Languages.lang[272] = result; //Text hướng dẫn
        }
    }

    /// <summary>
    /// Lấy số búa còn lại
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static IEnumerator BreakBall_GetHammerRemaining(string userID)
    {
        APIState = State.Waiting;
        var www = UnityWebRequest.Get(ServerLink + "BreakBallGet.php?id=" + userID);
        www.SetRequestHeader("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        if (www.isNetworkError || www.isHttpError)
        {
            APIState = State.LostConnected;
        }
        else
        {
            //int number = 0;
            //if (int.TryParse(result, out int number))
            //{
            //    BreakBall_HammerRemaining = Convert.ToInt32(result);
            //    APIState = State.Success;
            //}
            //else //Nếu server không trả về dc số nguyên thì failed
            //    APIState = State.Failed;
        }
    }

    /// <summary>
    /// Thực hiện việc đập bóng
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public static IEnumerator BreakBall_BreakBallAtion(string userID)
    {
        APIState = State.Waiting;
        var www = UnityWebRequest.Get(ServerLink + "BreakBallAction.php?id=" + userID);
        www.SetRequestHeader("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        if (www.isNetworkError || www.isHttpError)
        {
            APIState = State.LostConnected;
        }
        else
        {
            if (result.StartsWith("OK"))
            {
                BreakBall_HammerRemaining = Convert.ToInt32(result.Replace("OK", ""));
                APIState = State.Success;
            }
            else //Nếu server không trả về dc số nguyên thì failed
                APIState = State.Failed;
        }
    }

    #endregion

    #region Chatbox và Feedback 

    /// <summary>
    /// Lấy dữ liệu từ chatbox
    /// </summary>
    /// <returns></returns>
    public static IEnumerator GetContentChatbox()
    {
        ChatboxString = "";
        UnityWebRequest www = UnityWebRequest.Get("https://xn--mten-1ua4066b.vn/Chatbox/get.php");
        www.SetRequestHeader("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
            ChatboxString = "-1";
        else
            ChatboxString = www.downloadHandler.text;
    }
    public static IEnumerator PostContentChatbox(string name, string content)
    {
        StatePostChatbox = "";
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("content", content);
        UnityWebRequest www = UnityWebRequest.Post("https://xn--mten-1ua4066b.vn/Chatbox/post.php", form);
        www.chunkedTransfer = true;
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            StatePostChatbox = "-1";
        }
        else
        {
            StatePostChatbox = "OK";
        }
    }

    #endregion

    #region Online Reward 

    /// <summary>
    /// Nhận thưởng khi trực tuyến
    /// </summary>
    /// <param name="userID"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerator OnlineRewardAction(string userID, int type)
    {
        APIState = State.Waiting;
        var www = UnityWebRequest.Get(ServerLink + "HourRewardAction.php?id=" + userID + "&type=" + type);
        www.SetRequestHeader("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
        yield return www.SendWebRequest();
        string result = www.downloadHandler.text;
        if (www.isNetworkError || www.isHttpError)
        {
            APIState = State.LostConnected;
        }
        else
        {
            if (result.StartsWith("OK") || result.StartsWith("Failed"))
            {
                if (result.StartsWith("Failed"))
                { //Trả về dữ liệu chưa thể nhận thưởng
                    GlobalVariables.OnlineRewardTimeRemaining = Convert.ToInt32(result.Split(';')[1]); //Số giây còn lại để nhận thưởng
                    GlobalVariables.OnlineRewardMoneyValue = Convert.ToInt32(result.Split(';')[2].Split(',')[1]); //Số tiền sẽ nhận đc
                    APIState = State.Success;
                }
                else if (result.StartsWith("OK"))
                { //Trả về dữ liệu chưa thể nhận thưởng
                    if (type.Equals(0)) //Kiểu get để xem
                    {
                        GlobalVariables.OnlineRewardTimeRemaining = 0; //Số giây còn lại để nhận thưởng
                        GlobalVariables.OnlineRewardMoneyValue = Convert.ToInt32(result.Split(';')[1].Split(',')[1]); //Số tiền sẽ nhận đc
                        APIState = State.Success;
                    }
                    else if (type.Equals(1)) //Kiểu nhận thưởng và get dữ liệu
                    {
                        GlobalVariables.OnlineRewardTimeRemaining = Convert.ToInt32(result.Split(';')[2]); //Số giây còn lại để nhận thưởng
                        GlobalVariables.OnlineRewardMoneyValue = Convert.ToInt32(result.Split(';')[3].Split(',')[1]); //Số tiền sẽ nhận đc lần tới
                        var reward = Convert.ToInt32(result.Split(';')[1].Split(',')[1]);
                        DataUserController.User.Gems += reward;
                        GameSystem.ControlFunctions.ShowMessage(Languages.lang[275] + reward + Languages.lang[277]);
                        APIState = State.Success;
                        DataUserController.SaveUserInfor();
                    }
                }
            }
            else
            {
                APIState = State.Failed;
            }
        }
    }
    #endregion

    public static IEnumerator APIGet(string url, string jsonData)
    {
        //APIState = State.Waiting;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
            APIState = State.LostConnected;
                GameSystem.ShowMessage(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                var yourObject = JsonUtility.FromJson<Player>(webRequest.downloadHandler.text);
                //Debug.Log(Securitys.Encrypt(webRequest.downloadHandler.text));
            }
        }
    }
    public static IEnumerator APIPut(string url, string jsonData)
    {
        var inventoryData = new InventoryDataModel { UserID = DataUserController.User.UserID, HWID = SystemInfo.deviceUniqueIdentifier.ToString(), Data = Securitys.Encrypt(JsonUtility.ToJson(DataUserController.Inventory)), LastUpdate = 1234567 };
        var jcon = JsonUtility.ToJson(inventoryData);

        UnityWebRequest www = UnityWebRequest.Put("http://localhost:12345/player/syncinventory/", jcon);
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            APIState = State.LostConnected;
            GameSystem.ShowMessage(www.error);
        }
        else
        {
            var result = www.downloadHandler.text;
           var a = JsonUtility.FromJson<ResponseModel>(result);
            Debug.Log(a.Res.ToString());
        }
    }

    public static IEnumerator APIPost(string url, string jsonData)
    {
        jsonData = "{\"userID\":\"fd45f\",\"userName\":null,\"golds\":0,\"gems\":50,\"inventorySlot\":0,\"battleWin\":0,\"battleLose\":0,\"numberSpined\":0,\"itemUserForBattle\":null,\"isAutoBattle\":false,\"enemyFutureMap\":null,\"difficultMap\":null,\"lastUpdate\":0,\"hwid\":null}";
        UnityWebRequest www = UnityWebRequest.Post("http://localhost:12345/player/tank/", "{\"player\":46546}");
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            APIState = State.LostConnected;
            GameSystem.ShowMessage(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}