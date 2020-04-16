using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;

public static class NetBase
{

    /// <summary>
    /// Check connection. False = not connection
    /// </summary>
    public static bool Connect;
    /// <summary>
    /// Server domain
    /// </summary>
    public static string DomainServer = "http://localhost/cgw/";
    //public static string DomainServer;
    /// <summary>
    /// Link target have Server domain
    /// </summary>
    static string LinkGetDomainServer = "https://docs.google.com/document/d/1mSeoGrWdHCGubGBMiQvfTDJSA8uQ1Gq0PoJVGKeNh1Q/edit?usp=sharing";
    
    /// <summary>
    /// 0: Version
    /// 1: Domain
    /// 3: Link download new version
    /// </summary>
    public static string[] NetBaseValue = new string[5];
    /// <summary>
    /// Get Server Domain
    /// </summary>
    /// <returns></returns>
    public static IEnumerator GetDomainServer()
    {
        UnityWebRequest www = UnityWebRequest.Get(LinkGetDomainServer);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Connect = false;
        }
        else
        {
            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            string[] TextTemp = www.downloadHandler.text.Split('~');//TextTemp[1] is base value (TextTemp[1] là chuỗi chứa toàn bộ thông tin data để connect API)
            TextTemp[1].Replace(";;", ";");
            NetBaseValue = TextTemp[1].Split(';');
            Connect = true;
        }
    }
    /// <summary>
    /// Sync User Data to Server
    /// </summary>
    /// <param name="IdUser"></param>
    /// <param name="DataString"></param>
    /// <returns></returns>
    public static IEnumerator Sync(int IdUser, string DataString)
    {
        if (DomainServer != "")
        {
            WWWForm form = new WWWForm();
            form.AddField("Id", IdUser);
            form.AddField("DataString", DataString);
            UnityWebRequest www = UnityWebRequest.Post(DomainServer + "UpdateUser.php", form);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);
            else
                Debug.Log(www.downloadHandler.text);
        }
    }
}
