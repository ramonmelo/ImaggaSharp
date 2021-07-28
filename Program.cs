using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoTagger
{
    class Program
    {
        private const string MAIN_API = "https://api.imagga.com/v2/tags";

        private const string API_KEY = "***";
        private const string API_SECRET = "***";

        private static readonly Dictionary<string, string> PARAMS = new Dictionary<string, string>() {
            { "verbose", "0" },
            { "threshold", "30.0" },
            { "language", "en,pt" }
        };

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var path = args[0];
                RequestTags(path);
            }
        }

        static void RequestTags(string filePath)
        {
            string basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", API_KEY, API_SECRET)));

            var client = new RestClient(MAIN_API)
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.POST);

            foreach (var item in PARAMS)
            {
                request.AddParameter(item.Key, item.Value);
            }

            request.AddFile("image", filePath);
            request.AddHeader("Authorization", string.Format("Basic {0}", basicAuthValue));

            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}
