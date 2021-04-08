using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.IO;

public class Client 
{
	public Client()
	{
        // This constructor arbitrarily assigns the local port number.
        UdpClient udpClient = new UdpClient(5400);
        try
        {
            udpClient.Connect("localhost", 5400);
            StreamReader s = new StreamReader(File.OpenRead("C:\\Users\\miche\\Desktop\\university\\cpp\\Tzvi-csharp\\Tzvi-csharp\\reg_flight.csv"));
            while (!s.EndOfStream)
            {
                var newline = s.ReadLine();
                String eol = "\n\r";
                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes(newline + eol);
                udpClient.Send(sendBytes, sendBytes.Length);
                Thread.Sleep(100);
            }
            udpClient.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
	}


}
