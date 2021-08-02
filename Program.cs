using System;
using System.Configuration;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text;

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
                    new string[] { "--file", "-f" },
                    description: "Path to photo to be analyzed")
                {
                    Arity = ArgumentArity.ExactlyOne
                },
                new Option<string[]>(
                    new string[] { "--language", "-l" },
                    getDefaultValue: () => new string[] { "en" },
                    description: "Language of the Tags")
                {
                    Arity = ArgumentArity.OneOrMore
                },
                new Option<int>(
                    "--limit",
                    getDefaultValue: () => 20,
                    description: "Max number of tags")
                {
                    Arity = ArgumentArity.ZeroOrOne
                },
                new Option<int>(
                    "--threshold",
                    getDefaultValue: () => 30,
                    description: "Threshold of confidence")
                {
                    Arity = ArgumentArity.ZeroOrOne
                },
            };

            tagCommand.Handler = CommandHandler.Create<FileInfo, string[], int, int>((file, language, limit, threshold) =>
            {
                if (file == null || file.Exists == false)
                {
                    Console.WriteLine($"Invalid Path or file does not exist.");
                    return;
                }

                Console.WriteLine($"Processing: {file.FullName}");

                api = new ImaggaAPI(config_api_key, config_api_secret);

                var result = api.GetTagsFromFile(file.FullName, language: language, limit: limit, threshold: threshold);

                if (result != null && result.status.IsSuccess)
                {
                    var tagList = new StringBuilder();

                    foreach (var item in result.result.tags)
                    {
                        foreach (var itemTag in item.tag)
                        {
                            tagList.Append($"#{itemTag.Value.Replace(" ", "")} ");
                        }
                    }

                    Console.WriteLine(tagList.ToString());
                }
            });

            rootCommand.Add(tagCommand);
            rootCommand.Description = "Console Application to interface with the Imagga API";

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
