namespace ChatgptAssistant
{
    public class AppSettings
    {
        public string OpenAIModelID { get; set; }
        public string OpenAIKey { get; set; }
        public string PromptDirectory { get; set; }
        public string ResultDirectory { get; set; }

        public AppSettings(string openAIModelId, string openAIKey, string promptDir, string resultDirectory)
        {
            OpenAIModelID = openAIModelId;
            OpenAIKey = openAIKey;
            PromptDirectory = promptDir;
            ResultDirectory = resultDirectory;
        }
    }

}
