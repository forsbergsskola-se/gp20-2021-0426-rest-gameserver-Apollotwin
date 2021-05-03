using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "acme.com";
            var port = 80;
            var tcpClient = new TcpClient(host, port);
            if (tcpClient.Connected)
            {
                // Send a HTTP-Request for the Root-Page
                var streamWriter = new StreamWriter(tcpClient.GetStream());
                streamWriter.Write("GET / HTTP/1.1\r\nHost: acme.com\r\n\r\n");
                streamWriter.Flush();
        
                // Read the Response
                var streamReader = new StreamReader(tcpClient.GetStream());
                var response = streamReader.ReadToEnd();
                Console.WriteLine(response);
                tcpClient.GetStream().Close();
                tcpClient.Close();

                var titleText = FindTextBetweenTags(response, "<title>", "</title>");
                Console.WriteLine("Retrived string: " +titleText);
            }
        }

        static string FindTextBetweenTags(string response, string start, string end)
        {
            var indexStart = response.IndexOf(start);
            var indexEnd = response.IndexOf(end);
            var title = string.Empty;
            if(indexStart != -1)
            {
                indexStart += start.Length;
                if (indexEnd > indexStart)
                {
                    title = response[indexStart..indexEnd];
                }
            }

            return title;
        }
    }
}
