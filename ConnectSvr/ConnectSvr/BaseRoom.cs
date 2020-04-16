using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class BaseRoom
{
    public int RoomID { get; set; }
    public string RoomName { get; set; }
    public bool IsOpen { get; set; }
    public bool IsStarted { get; set; }
    public int UserInRoom { get; set; }
    public string UserPrimary { get; set; }
    public string[] UserName { get; set; }
    public Socket[] Soc { get; set; }
    public string Password { get; set; }
    public string[] CharID { get; set; }
}


