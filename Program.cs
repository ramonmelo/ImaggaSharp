using System;

namespace PhotoTagger
{
    class Program
    {
        private const string API_KEY = "";
        private const string API_SECRET = "";

        private static ImaggaAPI api;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var path = args[0];

                api = new ImaggaAPI(API_KEY, API_SECRET);
                var result = api.GetTagsFromFile(path, language: new string[] { "en" }, limit: 15, threshold: 40f);

                if (result != null && result.status.IsSuccess)
                {
                    foreach (var item in result.result.tags)
                    {
                        Console.WriteLine(item.confidence);

                        foreach (var itemTag in item.tag)
                        {
                            Console.WriteLine($"{itemTag.Key}: {itemTag.Value}");
                        }
                    }
                }
            }
        }
    }
}
