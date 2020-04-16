using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using BlackCore;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
public static class SyncData {
    /// <summary>
    /// Check connection. False = not connection
    /// </summary>
    public static bool Connect;
    /// <summary>
    /// Server domain (link phải có /formResponse như dưới, thì mới submit dc gg form)
    /// </summary>
    public static readonly string DomainServer = "https://docs.google.com/forms/d/e/1FAIpQLScu7ffkH02-3jyGcvhHM4EMNuZoHBg0FUBVavW4Z_3llk_ZtQ/formResponse";
    public static string ServerString = "";
    public static readonly string HeroBalanceLink = "https://docs.google.com/forms/d/e/1FAIpQLSc-u0k5VoyDfQEr6y48DY-z7p_PEyQquyef4i9s3fTalR-blQ/formResponse"; //
    //public stat readonlyic string DomainServer = "http://localhost/SvrHL/";
    public static string ChatboxString = "";
    public static string StatePostChatbox = ""; //Trạng thái post chatbox dc hay chưa
    //public static string DomainServer;
    /// <summary>
    /// Link target have Server domain
    /// </summary>
    static readonly string LinkGetDomainServer = "https://docs.google.com/document/d/1mSeoGrWdHCGubGBMiQvfTDJSA8uQ1Gq0PoJVGKeNh1Q/edit?usp=sharing";
    private static readonly string LinkGetInteretTime = "http://www.microsoft.com";
    public static DateTime? RealTime = null;
    public static State stat;
    public enum State {
        Waiting,
        Success,
        ConnectOK,
        LostConnect,
    }

    #region Khởi tạo lấy dữ liệu từ server 

    // <summary>
    /// Get Server Domain
    /// </summary>
    /// <returns></returns>
    public static IEnumerator GetServerString () {
        ServerString = "";
        UnityWebRequest www = UnityWebRequest.Get (LinkGetDomainServer);
        yield return www.SendWebRequest ();

        if (www.isNetworkError || www.isHttpError) {
            Connect = false;
            ServerString = "-1";
        } else {
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            string[] TextTemp = www.downloadHandler.text.Split ('~');
            ServerString = TextTemp[1];
            API.ServerLink = ServerString.Split (';') [4]; //Link server
            API.APIState = API.State.Connected;
            Debug.Log (ServerString);
            Connect = true;
        }
    }

    /// <summary>
    /// Lấy thời gian online
    /// </summary>
    /// <returns></returns>
    public static IEnumerator GetInternetTime () {
        RealTime = null;
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get (LinkGetInteretTime);
        yield return myHttpWebRequest.SendWebRequest ();

        string netTime = myHttpWebRequest.GetResponseHeader ("date");
        RealTime = !string.IsNullOrEmpty (netTime) ? Convert.ToDateTime (netTime).ToUniversalTime () : RealTime;
        Debug.Log (RealTime);
        //Debug.Log (RealTime.Value < Convert.ToDateTime ("09/15/2019 06:28:09").ToUniversalTime () ? "true" : "false");
    }

    #endregion

    #region Chatbox và Feedback 

    /// <summary>
    /// Lấy dữ liệu từ chatbox
    /// </summary>
    /// <returns></returns>
    public static IEnumerator GetContentChatbox () {
        ChatboxString = "";
        // UnityWebRequest www = UnityWebRequest.Get (ServerString.Split (';') [2]);
        UnityWebRequest www = UnityWebRequest.Get ("https://xn--mten-1ua4066b.vn/Chatbox/get.php");
        www.SetRequestHeader ("user-agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36");
        yield return www.SendWebRequest ();

        if (www.isNetworkError || www.isHttpError)
            ChatboxString = "-1";
        else
            ChatboxString = www.downloadHandler.text;
    }
    public static IEnumerator PostContentChatbox (string name, string content) {
        StatePostChatbox = "";
        WWWForm form = new WWWForm ();
        form.AddField ("name", name);
        form.AddField ("content", content);
        // UnityWebRequest www = UnityWebRequest.Post (ServerString.Split (';') [3], form);
        UnityWebRequest www = UnityWebRequest.Post ("https://xn--mten-1ua4066b.vn/Chatbox/post.php", form);
        www.chunkedTransfer = true;
        yield return www.SendWebRequest ();
        if (www.isNetworkError || www.isHttpError) {
            //Debug.Log(www.error);
            StatePostChatbox = "-1";
        } else {
            //Debug.Log(www.downloadHandler.text);
            StatePostChatbox = "OK";
        }
    }

    public static IEnumerator Feedback (string _subject, string _note, string _email) {
        if (!string.IsNullOrEmpty (DomainServer)) {
            WWWForm form = new WWWForm ();
            form.AddField ("entry.1345782542", SystemInfo.deviceUniqueIdentifier);
            form.AddField ("entry.1040192494", System.DateTime.Today.ToString ());
            form.AddField ("entry.1003935287", _subject);
            form.AddField ("entry.1501174593", _note);
            form.AddField ("entry.1157880709", _email);
            UnityWebRequest www = UnityWebRequest.Post (DomainServer, form);
            yield return www.SendWebRequest ();
            stat = (www.isNetworkError || www.isHttpError) ? State.LostConnect : State.Success;
        }
    }

    #endregion

    #region Phần admin 

    /// <summary>
    /// Submit dữ liệu hero default lên form
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static IEnumerator HeroBlanceDefault (string data) {
        if (!string.IsNullOrEmpty (HeroBalanceLink)) {
            WWWForm form = new WWWForm ();
            form.AddField ("entry.1523124801", SystemInfo.deviceUniqueIdentifier);
            form.AddField ("entry.1699953446", data);
            UnityWebRequest www = UnityWebRequest.Post (HeroBalanceLink, form);
            yield return www.SendWebRequest ();
            stat = (www.isNetworkError || www.isHttpError) ? State.LostConnect : State.Success;
        }
    }

    #endregion

    /// <summary>
    /// Sync User Data to Server
    /// </summary>
    /// <param name="IdUser"></param>
    /// <param name="DataString"></param>
    /// <returns></returns>
    public static IEnumerator Sync (int IdUser, string DataString) {
        if (DomainServer != "") {
            WWWForm form = new WWWForm ();
            form.AddField ("Id", IdUser);
            form.AddField ("DataString", DataString);
            UnityWebRequest www = UnityWebRequest.Post (DomainServer + "UpdateUser.php", form);
            yield return www.SendWebRequest ();
            if (www.isNetworkError || www.isHttpError)
                Debug.Log (www.error);
            else
                Debug.Log (www.downloadHandler.text);
        }
    }
}