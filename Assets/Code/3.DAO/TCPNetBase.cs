using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;
public static class TCPNetBase
{
    public static string Teststring { get; set; }
    public static string Teststring2 { get; set; }
    //Base Room
    public static int RoomID { get; set; }
    public static string RoomName { get; set; }
    public static bool IsOpen { get; set; }
    public static bool IsStarted { get; set; }
    public static int UserInRoom { get; set; }
    public static string UserPrimary { get; set; }

    public static string[] UserName = new string[2];

    public static string[] CharID = new string[2];
    public static Socket[] Soc { get; set; }
    public static string Password { get; set; }
    static Ping myPing = new Ping();
    public static PingReply PingStatus;
    public static int CountWait;
    public static int CountWaitMax = 5;

    public static bool IsRefreshData;
    public static bool IsOurRoom;
    public static bool IsRefreshMessenger;
    public static string MessengerServer;

    public static bool IsKeyRoom;//Check xem có phải là chủ phòng hay không

    //Join room
    public static bool HavePlayerJoinRoom;
    public static string InforPlayerJoinRoom;

    public static bool ShowPing;
    public static bool IsRefreshPing;
    public static long PingValue;
    //TCP Protocol connect variable
    private const int BUFFER_SIZE = 1024;
    public const int PORT_NUMBER = 1306;
    public static TcpClient client;
    public static Stream stream;
    public static bool Connected;
    public static ASCIIEncoding encoding = new ASCIIEncoding();
    public static string IP = "103.130.213.7";
    //public static string IP = "192.168.1.77";
    public static string aaa;
    public static bool LoadConnection()
    {
        try
        {
            // 1. connect
            client = new TcpClient();
            client.Connect(IP, PORT_NUMBER);
            stream = client.GetStream();
            var readercheck = new StreamReader(stream);
            readercheck.BaseStream.ReadTimeout = 1000;
            var checkconn = new StreamWriter(stream);
            checkconn.Write("Connect");
            //checkconn.WriteLine("requestconnect");
            //textBox1.Text = "Do not connect to Server";
            Connected = false;
            if (readercheck.ReadLine() == "OK")
            {
                Connected = true;
                //textBox1.Text = "Connect Successfully";
            }
            else
            {
                Connected = false;
                //textBox1.Text = ("Do not connect to Server");
            }
            return Connected;
            // 4. close
            //stream.Close();
            //client.Close();
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
            return false;
        }
    }
    public static string ReciveData(string command)
    {
        var reader = new StreamReader(stream);
        var writer = new StreamWriter(stream);
        writer.AutoFlush = true;
        writer.WriteLine(command);
        return reader.ReadLine();
    }

    public static string WaitData()
    {
        while (Connected)
        {
            try
            {
                return ReciveData("InfoRoom");
            }
            catch
            {
                if (CountWait > CountWaitMax)
                {
                    stream.Close();
                    client.Close();
                    Connected = false;
                }
            }
        }
        return null;
    }
    public static string CreateRoom()
    {
        string command = "CreateRoom;" + UnityEngine.SystemInfo.deviceUniqueIdentifier + ";" + Module.GameLoad("CharID");
        var reader = new StreamReader(stream);
        var writer = new StreamWriter(stream);
        writer.AutoFlush = true;
        writer.WriteLine(command);
        return reader.ReadLine();
    }
    public static string JoinRoom(string roomid)
    {
        string command = "JoinRoom;" + UnityEngine.SystemInfo.deviceUniqueIdentifier + ";" + roomid + ";" + Module.GameLoad("CharID");
        var reader = new StreamReader(stream);
        var writer = new StreamWriter(stream);
        writer.AutoFlush = true;
        writer.WriteLine(command);
        return reader.ReadLine();
    }
    public static void OutRoom()
    {
        try
        {
            string command = "OutRoom;";
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            writer.WriteLine(command);
            stream.Close();
            client.Close();
            Connected = false;
            IsOurRoom = true;
            IsRefreshData = false;
            MessengerServer = "";
            CountWait = 0;
            IsKeyRoom = false;
            ClearData();
        }
        catch { }
    }
    public static void LoopPing()
    {
        while (Connected)
        {
            PingStatus = myPing.Send(IP, 1000);
            PingValue = PingStatus.RoundtripTime;
            IsRefreshPing = true;
            Thread.Sleep(1000);
        }
        IsRefreshPing = false;
    }

    public static void MappingData(string datastring)
    {
        var strdata = datastring.Split(';');
        RoomID = Convert.ToInt32(strdata[1]);
        UserInRoom = Convert.ToInt32(strdata[2]);
        UserName[0] = strdata[3];
        UserName[1] = strdata[4];
        CharID[0] = strdata[5];
        CharID[1] = strdata[6];
    }
    public static void ClearData()
    {
        RoomID = 0;
        UserInRoom = 0;
        UserName[0] = "";
        UserName[1] = "";
        CharID[0] = "";
        CharID[1] = "";
    }
}

