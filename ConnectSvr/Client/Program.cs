using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

public class Program
{

    private const int BUFFER_SIZE = 1024;
    private const int PORT_NUMBER = 1306;
    private const string IP = "127.0.0.1";
    static ASCIIEncoding encoding = new ASCIIEncoding();
    static void Action()
    {

    }
    public static void Main()
    {

        try
        {
            TcpClient client = new TcpClient();

            // 1. connect
            client.Connect(IP, PORT_NUMBER);
            Stream stream = client.GetStream();
            var readercheck = new StreamReader(stream);
            var checkconn = new StreamWriter(stream);
            checkconn.Write("requestconnect");
            //checkconn.WriteLine("requestconnect");
            Console.Write("Wait for connect to Server\n");
            if (readercheck.ReadLine() == "OK")
            {
                Console.Write("Connect Successfully!\n");
                while (true)
                {
                    Console.Write("Enter your name: ");

                    var reader = new StreamReader(stream);
                    var writer = new StreamWriter(stream);

                    string str = Console.ReadLine();
                    writer.AutoFlush = true;

                    // 2. send
                    //writer.WriteLine(str);

                    // 3. receive
                   str = reader.ReadLine();
                    Console.WriteLine(str);
                    //if (str.ToUpper() == "BYE")
                        break;
                }
            }
            else
            {
                Console.Write("Do not connect to Server");
            }
                Console.Write("Do not connect to Server");
            // 4. close
            stream.Close();
            client.Close();
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex);
        }

        Console.Read();
    }
}