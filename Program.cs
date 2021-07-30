using System;
using System.Configuration;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace PhotoTagger
{
    class Program
    {
        private static ImaggaAPI api;

        static int Main(string[] args)
        {
            string config_api_key = ConfigurationManager.AppSettings["api_key"];
            string config_api_secret = ConfigurationManager.AppSettings["api_secret"];

            if (string.IsNullOrEmpty(config_api_key))
            {
                Console.WriteLine("Invalid API Key");
                return 0;
            }

            if (string.IsNullOrEmpty(config_api_secret))
            {
                Console.WriteLine("Invalid API Secret");
                return 0;
            }

            // Create a root command with some options
            var rootCommand = new RootCommand();

            var tagCommand = new Command("tag")
            {
                new Option<FileInfo>(
                    "--file",
                    description: "Path to photo to be analyzed"),
            };

            tagCommand.Handler = CommandHandler.Create<FileInfo>((file) =>
            {
                if (file.Exists == false)
                {
                    Console.WriteLine($"Invalid Path: {file.FullName}");
                    return;
                }

                Console.WriteLine($"Processing: {file.FullName}");

                api = new ImaggaAPI(config_api_key, config_api_secret);

                var result = api.GetTagsFromFile(file.FullName, language: new string[] { "en" }, limit: 15, threshold: 40f);

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
            });

            rootCommand.Add(tagCommand);
            rootCommand.Description = "Console Application to interface with the Imagga API";

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
