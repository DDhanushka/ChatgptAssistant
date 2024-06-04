﻿using Microsoft.Extensions.Configuration;

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
    }
}