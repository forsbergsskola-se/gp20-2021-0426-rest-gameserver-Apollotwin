using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GitHubExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter a user you want to inspect: ");
            string username = "Apollotwin";
            string myToken = "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";
            string header = $"Authorization: token {myToken}";
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/users/{username}");
            httpClient.DefaultRequestHeaders.Add("Authorization",$"token {myToken}" );
            httpClient.Send(request);
            Console.WriteLine(request);
            var response = new HttpResponseMessage();
            var stream = new StreamReader(response.Content.ReadAsStream());
            Console.WriteLine(response);
            Console.WriteLine(response.Headers);
            Console.WriteLine(stream.ReadToEnd());
            var streamToJson = JsonSerializer.Deserialize<string>(stream.ReadToEnd());
            Console.WriteLine(streamToJson);
            //Console.WriteLine(request.Content);
        }
    }
}
