using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Threading;

namespace ClientWinForm
{
    public partial class Form1 : Form
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 9999;
        TcpClient client = new TcpClient();
        Stream stream;
        bool Connected;
        static ASCIIEncoding encoding = new ASCIIEncoding();
        public Form1()
        {
            InitializeComponent();
            new Thread(LoadConnection).Start();
            //LoadConnection();
        }
        void LoadConnection()
        {
            try
            {

                // 1. connect
                client.Connect("127.0.0.1", PORT_NUMBER);
                
                stream = client.GetStream();
                var readercheck = new StreamReader(stream);
                var checkconn = new StreamWriter(stream);
                checkconn.Write("requestconnect");
                //checkconn.WriteLine("requestconnect");
                textBox1.Text = "Do not connect to Server";
                Connected = false;
                if (readercheck.ReadLine() == "OK")
                {
                    Connected = true;
                   // Console.Write("Connect Successfully!\n");
                    textBox1.Text = "Connect Successfully";
                    //while (true)
                    //{
                    //    Console.Write("Enter your name: ");

                    //    var reader = new StreamReader(stream);
                    //    var writer = new StreamWriter(stream);

                    //    string str = Console.ReadLine();
                    //    writer.AutoFlush = true;

                    //    // 2. send
                    //    writer.WriteLine(str);

                    //    // 3. receive
                    //    str = reader.ReadLine();
                    //    Console.WriteLine(str);
                    //    if (str.ToUpper() == "BYE")
                    //        break;
                    //}
                }
                else
                {
                    textBox1.Text = ("Do not connect to Server");
                }
                // 4. close
                //stream.Close();
                //client.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
        string ReciveData(string command)
        {
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            writer.WriteLine(command);
            return reader.ReadLine();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                try
                {
                    textBox1.Text = ReciveData(textBox1.Text);
                    if (textBox1.Text.ToUpper() == "BYE")
                    {
                        stream.Close();
                        client.Close();
                        Application.Exit();
                    }
                }
                catch
                {
                    stream.Close();
                    client.Close();
                textBox1.Text = ("Do not connect to Server");
                }
            }
             else
                textBox1.Text = ("Do not connect to Server");
        }
    }
}
