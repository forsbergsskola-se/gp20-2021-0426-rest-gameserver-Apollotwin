using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GitHubExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = Console.ReadLine();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add($"https://api.github.com/users/{username}", username);
        }
    }
}
