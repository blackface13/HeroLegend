using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.Networking;

//namespace Assets.Code.NetWork

public static class LANBase
{
    public static bool _isStarted = false;
    public static bool _isServer = false;
    public static string ip = "192.168.0.136";
    public static int port = 100;
    public static int _messageIdx = 0;

    public static int m_ConnectionId = 0;
    public static int m_WebSocketHostId = 0;
    public static int m_GenericHostId = 0;

    public static string m_SendString = "";
    public static string m_RecString = "";
    public static ConnectionConfig m_Config = null;
    public static byte m_CommunicationChannel = 0;
    public static int PortClient;

    public static void Start()
    {
        m_Config = new ConnectionConfig();                                         //create configuration containing one reliable channel
        m_CommunicationChannel = m_Config.AddChannel(QosType.Reliable);
        //Txt[0].text = ip + ":" + port.ToString();
        //Inp.text = port.ToString();
    }
    public static bool StartServer()
    {
        port = UnityEngine.Random.Range(1111, 9999);
        _isStarted = true;
        _isServer = true;
        NetworkTransport.Init();

        HostTopology topology = new HostTopology(m_Config, 12);
        m_WebSocketHostId = NetworkTransport.AddWebsocketHost(topology, port, null);           //add 2 host one for udp another for websocket, as websocket works via tcp we can do this
        m_GenericHostId = NetworkTransport.AddHost(topology, port, null);
        return true;
    }
    public static void StartClient(int portinput)
    {
        port = portinput;
        _isStarted = true;
        _isServer = false;
        NetworkTransport.Init();

        HostTopology topology = new HostTopology(m_Config, 12);
        m_GenericHostId = NetworkTransport.AddHost(topology, 0); //any port for udp client, for websocket second parameter is ignored, as webgl based game can be client only
        byte error;
        m_ConnectionId = NetworkTransport.Connect(m_GenericHostId, ip, port, 0, out error);
    }
    public static void Stop()
    {
        {
            _isStarted = false;
            NetworkTransport.Shutdown();
            //Txt[0].text = ip + ":" + port.ToString();
            //Txt[1].text = "";
            //Txt[2].text = "You are not connect";
        }
    }
}

