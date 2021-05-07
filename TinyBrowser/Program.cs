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
                var streamWriter = new StreamWriter(tcpClient.GetStream());
                streamWriter.Write("GET / HTTP/1.1\r\nHost: acme.com\r\n\r\n");
                streamWriter.Flush();
                
                var streamReader = new StreamReader(tcpClient.GetStream());
                var response = streamReader.ReadToEnd();
                Console.WriteLine(response);
                tcpClient.GetStream().Close();
                tcpClient.Close();
                var index = 0;
                var titleText = FindTextBetweenTags(response, "<title>", "</title>", ref index);
                Console.WriteLine("Retrieved: " +titleText);
                int j = 1;
                for (int i = 0; i < response.Length; i++)
                {
                    var text = FindTextBetweenTags(response, "<a href=" + '"' + "mailto" + ':', '"'+">", ref index);
                    Console.WriteLine(j+": " + text + " (the url should be here)");
                    j++;
                }
                
            }
        }

        static string FindTextBetweenTags(string response, string start, string end, ref int startIndex)
        {
            var indexStart = response.IndexOf(start, startIndex);
            var indexEnd = response.IndexOf(end, indexStart);
            var title = string.Empty;
            if(indexStart != -1)
            {
                indexStart += start.Length;
                if (indexEnd > indexStart)
                {
                    title = response[indexStart..indexEnd];
                }
            }

            startIndex = indexEnd;
            
            return title;
        }
    }
}
