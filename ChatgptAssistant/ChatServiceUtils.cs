using ChatgptAssistant.Templates;
using Markdig;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace ChatgptAssistant;
public class ChatServiceUtils
{

    public static async Task StartChatAsync(IChatCompletionService chatGPT)
    {
        try
        {
            var chatHistory = new ChatHistory("You are a helpful assistant.");

            while (true)
            {
                Console.WriteLine("Your message: ");
                string? msg = Console.ReadLine();

                // Check if the user wants to exit
                if (msg?.ToLower() == "exit")
                {
                    // Instruct ChatGPT to create a title for the chat
                    chatHistory.AddUserMessage("Generate a title for this chat session. Title should be able to use as a file name in windows OS. Don't use - and _ to seperate words. But spaces");
                    var reply = await chatGPT.GetChatMessageContentAsync(chatHistory);
                    chatHistory.Add(reply);

                    break;
                }

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    // User sends msg
                    chatHistory.AddUserMessage(msg);

                    // GPT sends reply
                    var reply = await chatGPT.GetChatMessageContentAsync(chatHistory);
                    chatHistory.Add(reply);
                    await Utils.MessageOutputAsync(chatHistory);
                }
            }

            // We use a dictionary to keep a track of messages sent by each role.
            var messageDictionary = new Dictionary<Guid, ChatBubble>();
            var markDigPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            foreach (var msg in chatHistory)
            {
                // Ignore initial system message
                if (msg.Role == AuthorRole.System)
                {
                    continue;
                }

                // ChatGPT sends responses in Markdown format. We can convert them into HTML using MarkDig library
                var htmlContent = Markdown.ToHtml(msg.Content!, markDigPipeline);
                messageDictionary.Add(Guid.NewGuid(), new ChatBubble() { Message = htmlContent, Role = msg.Role.ToString() });
            }

            // Remove title generation message and reply so that they are not included in html output.
            var keysToRemove = messageDictionary.Keys.TakeLast(2).ToList();
            foreach (var key in keysToRemove)
            {
                messageDictionary.Remove(key);
            }

            // Last response from Chatgpt is the title for the chat session.
            var chatTitle = chatHistory.Last().Content?.Replace('"', ' ').Trim();

            // Pass message dictionary and title to the template
            var chatSessionTemplate = new ChatSessionTemplate
            {
                ChatHistory = messageDictionary,
                Title = chatTitle
            };

            // We can get the result directory from appsettings.json. User can change the directory as he wishes.
            var resultDirecotoryPath = Utils.GetAppSettings().ResultDirectory;

            var htmlPath = $"{resultDirecotoryPath}/{chatTitle}.html";
            File.WriteAllText(htmlPath, chatSessionTemplate.TransformText());

            // Open the saved html file with default browser.
            Utils.OpenFile(htmlPath);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }

    public static async Task RunCustomPrompt(IChatCompletionService chatGPT)
    {
        // Read the CustomePrompt.md
        var promptPath = Path.Combine(Utils.GetAppSettings().PromptDirectory, "CustomPrompt.md");
        var prompt = Utils.ReadFile(promptPath);

        // Create the result directory with a uniqe name
        var timeStamp = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssfff");
        var uniqueResultDirectory = Path.Combine(Utils.GetAppSettings().ResultDirectory, $"{timeStamp}");
        Directory.CreateDirectory(uniqueResultDirectory);

        // Create a copy of the input prompt
        var outputPath = Path.Combine(uniqueResultDirectory, $"{timeStamp}_CustomPrompt.md");
        File.WriteAllText(outputPath, prompt);

        // Start the chat session
        Console.WriteLine("Chat content:");
        Console.WriteLine("------------------------");

        // System message
        var chatHistory = new ChatHistory("You are an helpful assistant");
        
        // Submit the prompt to ChatGPT
        chatHistory.AddUserMessage(prompt);
        await Utils.MessageOutputAsync(chatHistory);

        // Response from ChatGPT
        var reply = await chatGPT.GetChatMessageContentAsync(chatHistory);
        chatHistory.Add(reply);
        await Utils.MessageOutputAsync(chatHistory);

    }
}

