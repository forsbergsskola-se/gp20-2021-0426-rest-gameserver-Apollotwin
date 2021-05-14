using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace TinyBrowser
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "acme.com";
            var port = 80;
            var response = string.Empty;
            var urlList = new List<string>();
            var defaultMessage = "";
            var lastMessage = string.Empty;
            var tcpClient = new TcpClient(host, port);
            var startOfProgram = true;
            int userInput = 0;

            
            
            while (true)
            {
                if (startOfProgram)
                {
                    tcpClient = new TcpClient(host, port);
                    response = SendAndReadMessage(tcpClient, defaultMessage);
                    lastMessage = defaultMessage;
                    urlList = GetUrlList(response);
                    ListURLAlternatives(urlList);
                    Console.WriteLine("Choose directory: ");
                    userInput = int.Parse(Console.ReadLine()); //TODO: Exeption handle this userinput
                    startOfProgram = false;
                }

                tcpClient = new TcpClient(host, port);
                response = SendAndReadMessage(tcpClient, urlList[userInput]);
                var saveMessage = urlList[userInput];
                urlList = GetUrlList(response);
                urlList.Add(lastMessage);
                lastMessage = saveMessage;
                ListURLAlternatives(urlList);
                Console.WriteLine("Choose directory: ");
                userInput = int.Parse(Console.ReadLine());

            }
        }

        private static void ListURLAlternatives(List<string> urlList)
        {
            for (int i = 0; i < urlList.Count; i++)
            {
                if (i == urlList.Count - 1 && urlList[i] == "") {
                    Console.WriteLine($"{i}: Back to {urlList[i]}");
                } else {
                    Console.WriteLine($"{i}: {urlList[i]}");
                }
                
            }
        }

        private static List<string> GetUrlList(string response)
        {
            var urlList = new List<string>();
            var textList = new List<string>();
            var index = 0;
            do
            {
                var localURL = FindTextBetweenTags(response, "<a href=" + '"', '"' + ">", ref index);
                if (localURL.Length != 0)
                    urlList.Add(localURL);
                var text = FindTextBetweenTags(response, ">", "</a>", ref index);
                if (text.Length != 0)
                    textList.Add(text);
            } while (index > 0);

            return urlList;
        }

        static string FindTextBetweenTags(string response, string start, string end, ref int startIndex)
        {
            var title = string.Empty;
            var indexStart = response.IndexOf(start, startIndex);
            if(indexStart != -1)
            {
                var indexEnd = response.IndexOf(end, indexStart);
                indexStart += start.Length;
                if (indexEnd > indexStart)
                {
                    title = response[indexStart..indexEnd];
                }
                startIndex = indexEnd;
            }
            return title;
        }
        
        static string SendAndReadMessage(TcpClient tcpClient, string message)
        {
            var streamWriter = new StreamWriter(tcpClient.GetStream());
            streamWriter.Write($"GET /{message} HTTP/1.1\r\nHost: acme.com\r\n\r\n");
            streamWriter.Flush();
            
            
            var streamReader = new StreamReader(tcpClient.GetStream());
            var response = streamReader.ReadToEnd();
            streamWriter.Dispose();
            streamReader.Dispose();
            return response;
        }
    }
}
