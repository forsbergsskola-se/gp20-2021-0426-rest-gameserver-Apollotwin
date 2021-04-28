using System;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpClient = new TcpClient("acme.com", 80);
            if (tcpClient.Connected)
            {
                Console.WriteLine("Connected");
                var stream = tcpClient.GetStream();
                stream.Write(Encoding.UTF8.GetBytes("GET \r\n"));
                var buffer = new byte[1024];
                var readStream = stream.Read(buffer, 0, buffer.Length);
                var stringBuilder = new StringBuilder().AppendFormat("{0}", Encoding.ASCII.GetString(buffer, 0, readStream));
                Console.WriteLine(stringBuilder);
                stream.Close();
                tcpClient.Close();
            }
            
        }
    }
}
