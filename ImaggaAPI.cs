using Newtonsoft.Json;
using RestSharp;
using System;
using System.Text;

namespace PhotoTagger
{
    internal class ImaggaAPI
    {
        private const string MAIN_API = "https://api.imagga.com/v2/";
        private const string API_TAGS = MAIN_API + "tags";

        private string _API_key = "";
        private string _API_secret = "";
        private string _API_AUTH = "";

        public ImaggaAPI(string key, string secret)
        {
            _API_key = key;
            _API_secret = secret;
            _API_AUTH = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", _API_key, _API_secret)));
        }

        public TagResult GetTagsFromFile(string filePath, string[] language = null, int limit = -1, float threshold = 7.0f)
        {
            if (language == null)
            {
                language = new string[] { "en" };
            }

            threshold = Math.Max(threshold, 7.0f);

            var jsonText = ExecutePost(API_TAGS, (request) =>
            {
                request.AddFile("image", filePath);

                request.AddParameter(nameof(limit), limit);
                request.AddParameter(nameof(threshold), threshold);
                request.AddParameter(nameof(language), string.Join(",", language));
            });

            if (jsonText != null)
            {
                return JsonConvert.DeserializeObject<TagResult>(jsonText);
            }

            return null;
        }

        private string ExecutePost(string url, Action<RestRequest> addParams)
        {
            var client = new RestClient(url)
            {
                Timeout = -1,
            };

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", string.Format("Basic {0}", _API_AUTH));

            addParams(request);

            IRestResponse response = client.Execute(request);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return response.Content;
                default:
                    break;
            }

            return null;
        }
    }
}
