using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Diagnostics;

namespace ChatgptAssistant
{
    internal class Utils
    {

        public static AppSettings GetAppSettings()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();


            string openAIModelId = configuration["OpenAIModelID"]!;
            string openAIKey = configuration["OpenAIKey"]!;
            string promptDirectory = configuration["PromptDirectory"]!;
            string resultDirectory = configuration["ResultDirectory"]!;

            AppSettings appSettings = new AppSettings(openAIModelId, openAIKey, promptDirectory, resultDirectory);
            return appSettings;
        }

        public static Task MessageOutputAsync(ChatHistory chatHistory)
        {
            var message = chatHistory.Last();

            if (message.Role == AuthorRole.Assistant)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine($"{message.Role}:\n{message.Content}");
            Console.ResetColor();
            Console.WriteLine("------------------------");

            return Task.CompletedTask;
        }

        public static void OpenFile(string path)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while opening file: {ex.Message}");
            }
        }

        public static string ReadFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return null;
            }
        }
    }
}
