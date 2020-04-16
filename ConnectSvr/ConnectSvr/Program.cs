using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.IO;
public class Program
{
    static int MAX_CONNECTION = 1000;
    const int PORT_NUMBER = 1306;
    //const string IP = "103.130.213.7";
    //const string IP = "192.168.1.77";
    const int RoomLimit = 1000;
    const int UserInRoom = 2;
    static BaseRoom[] Room = new BaseRoom[RoomLimit];
    static TcpListener listener;
    static int MINIDROOM = 111111;
    static int MAXIDROOM = 999999;
    public static void Main()
    {
        string[] Config = File.ReadAllLines(@"Config.cfg");
        if (Config.Length < 3)
        {
            Console.WriteLine("Miss information connection, check file Config");
            Console.Read();
        }
        else
        {
            MAX_CONNECTION = Convert.ToInt32(Config[2].Replace("MaxConnect=", ""));
            for (int i = 0; i < RoomLimit; i++)
                Room[i] = new BaseRoom();
            //Test room open
            //Room[5].IsOpen = true;
            //Room[5].UserName = new string[2];
            //Room[5].CharID = new string[2];
            //Room[5].RoomID = 999999;
            //Room[5].UserInRoom++;
            //Room[5].Soc = new Socket[2];
            //=================
            IPAddress address = IPAddress.Parse(Config[0].Replace("IP=", ""));

            listener = new TcpListener(address, Convert.ToInt32(Config[1].Replace("Port=", "")));
            Console.WriteLine("Server IP: " + Config[0].Replace("IP=", ""));
            Console.WriteLine("Server Port: " + Config[1].Replace("Port=", ""));
            Console.WriteLine("Server Started");
            listener.Start();

            for (int i = 0; i < MAX_CONNECTION; i++)
            {
                new Thread(Action).Start();
            }
        }
    }
    void CheckConnection()
    {

    }
    enum CMD
    {
        Login, Logout, CreateRoom, JoinRoom, OutRoom
    }
    private static void SendToClient(Socket socket, string mes)
    {
        var stream = new NetworkStream(socket);
        var writer = new StreamWriter(stream);
        writer.WriteLine(mes);
        writer.AutoFlush = true;
        socket.Close();
    }
    static void RegRoom(int slot, string usename, Socket soc, string charid)
    {
        Room[slot].IsOpen = true;
        Room[slot].RoomID = new Random().Next(MINIDROOM, MAXIDROOM);
        Room[slot].UserInRoom++;
        Room[slot].UserPrimary = usename;
        Room[slot].UserName = new string[2];
        Room[slot].UserName[0] = usename;
        Room[slot].Soc = new Socket[2];
        Room[slot].Soc[0] = soc;
        Room[slot].CharID = new string[2];
        Room[slot].CharID[0] = charid;
    }
    static void Action()
    {
        while (true)
        {
            Socket soc = listener.AcceptSocket();
            Console.WriteLine("Connection received from: {0}",
                              soc.RemoteEndPoint);
            var stream = new NetworkStream(soc);
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            var RoomInfo = new BaseRoom();
            //if (reader.ReadLine().Equals("Connect"))
            writer.WriteLine("OK");
            try
            {
                writer.AutoFlush = true;

                while (true)
                {
                                //writer.WriteLine("BlackFace");
                    string str = reader.ReadLine();
                    //Structure string send to server from client: 
                    //command   username   charid
                    if (CMD.CreateRoom.ToString().Equals(str.Split(';')[0]))
                    {
                        for (int i = 0; i < RoomLimit; i++)
                        {
                            if (!Room[i].IsOpen)//Reg Room if it not open
                            {
                                Room[i].IsOpen = true;
                                Room[i].RoomID = new Random().Next(MINIDROOM, MAXIDROOM);
                                Room[i].UserInRoom++;
                                Room[i].UserPrimary = str.Split(';')[1];
                                Room[i].UserName = new string[2];
                                Room[i].UserName[0] = str.Split(';')[1];
                                Room[i].Soc = new Socket[2];
                                Room[i].Soc[0] = soc;
                                Room[i].CharID = new string[2];
                                Room[i].CharID[0] = str.Split(';')[2];
                                RoomInfo = Room[i];
                                writer.WriteLine(str.Split(';')[1] + ";" + RoomInfo.RoomID);//Return UserName and RoomID
                                //SendToClient(soc);
                                break;
                            }
                            if (i >= RoomLimit - 1)
                                writer.WriteLine("Faild");
                        }
                    }
                    //Structure command join room: command; ID User; ID room; CharID
                    else if (CMD.JoinRoom.ToString().Equals(str.Split(';')[0]))
                    {
                        int roomslot = GetRoomSlot(Convert.ToInt32(str.Split(';')[2]));
                        if (roomslot != -1)
                        {
                            if (Room[roomslot].IsOpen)//Reg Room if it not open
                            {
                                if (Room[roomslot].UserInRoom >= 2)
                                {
                                    writer.WriteLine("Full");
                                    break;
                                }
                                Room[roomslot].UserInRoom++;
                                Room[roomslot].UserName[1] = str.Split(';')[1];
                                Room[roomslot].Soc[1] = soc;
                                Room[roomslot].CharID[1] = str.Split(';')[3];
                                RoomInfo = Room[roomslot];
                                writer.WriteLine("Success! Your Room is " + roomslot);
                                //SendToClient(Room[roomslot].Soc[0], "JoinRoom;" + Room[roomslot].UserName[1] + ";" + Room[roomslot].CharID[1]);
                            }
                            else
                                writer.WriteLine("Faild");
                        }
                        else
                            writer.WriteLine("Faild");
                    }
                    //Structure command out room: command; ID User; ID room
                    else if (CMD.OutRoom.ToString().Equals(str.Split(';')[0]))
                    {
                        goto Finish;
                    }
                    else
                    {
                        switch (str)
                        {
                            case "InfoRoom"://-------0----------------1-----------------------2---------------------------3-----------------------------4---------------------------5----------------------------6
                                writer.WriteLine("InfoRoom;" + RoomInfo.RoomID + ";" + RoomInfo.UserInRoom + ";" + RoomInfo.UserName[0] + ";" + RoomInfo.UserName[1] + ";" + RoomInfo.CharID[0] + ";" + RoomInfo.CharID[1]);
                                break;
                            case "FindRoom":
                                int count = 0;
                                int[] roomlist = new int[10];
                                for (int i = 0; i < RoomLimit; i++)
                                {
                                    if (Room[i].IsOpen)
                                    {
                                        roomlist[count] = Room[i].RoomID;
                                        if (count >= 10)
                                            break;
                                        count++;
                                    }
                                }
                                string resultroomlist = "";
                                for (int i = 0; i < count; i++)
                                    resultroomlist += roomlist[i] + "~";
                                writer.WriteLine("FindRoom;" + count + "; " + resultroomlist);
                                break;
                            default: break;
                        }
                    }
                    // 3. send
                    //writer.WriteLine("Hello " + str);
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex);
                //Console.WriteLine("Error: Client will disconnect.");
                stream.Close();
            }
        Finish:
            try
            {
                Console.WriteLine("Client disconnected: {0}",
                                  soc.RemoteEndPoint);
                if (RoomInfo.Soc[0].Equals(soc))
                {
                    if (!String.IsNullOrEmpty(RoomInfo.Soc[1].ToString()))
                    {
                        RemoveClient(RoomInfo.Soc[1]);
                        //RoomInfo.Soc[1].Close();
                    }
                }
                RemoveClient(soc);
                soc.Close();
            }
            catch
            {
                RemoveClient(soc);
                soc.Close();
            }
        }
    }
    static int GetRoomSlot(int roomid)
    {
        for (int i = 0; i < RoomLimit; i++)
            if (Room[i].RoomID.Equals(roomid))
            {
                return i;
            }
        return -1;
    }
    static void RemoveClient(Socket socket)
    {
        for (int i = 0; i < RoomLimit; i++)
        {
            if (Room[i].Soc != null)
            {
                if (Room[i].Soc[0] != null)//Nếu chủ phòng out
                    if (Room[i].Soc[0].Equals(socket))
                    {
                        ClearRoom(i);
                        break;
                    }
                if (Room[i].Soc[1] != null)//Nếu người tham gia out
                    if (Room[i].Soc[1].Equals(socket))
                    {
                        Room[i].Soc[1] = null;
                        Room[i].UserName[1] = "";
                        Room[i].UserInRoom--;
                        break;
                    }
            }
        }
    }
    static void ClearRoom(int roomslot)
    {
        Room[roomslot].IsOpen = false;
        Room[roomslot].RoomID = 0;
        if (Room[roomslot].Soc[0] != null)
        {
            Room[roomslot].Soc[0].Close();
            Room[roomslot].Soc[0] = null;
        }
        if (Room[roomslot].Soc[1] != null)
        {
            Room[roomslot].Soc[1].Close();
            Room[roomslot].Soc[1] = null;
        }
        Room[roomslot].UserInRoom = 0;
        Room[roomslot].UserName[0] = "";
        Room[roomslot].UserName[1] = "";
        Room[roomslot].UserPrimary = "";
    }
}