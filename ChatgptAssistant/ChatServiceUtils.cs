using Microsoft.SemanticKernel.ChatCompletion;

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
                    chatHistory.AddUserMessage("Generate a title for this chat session.");
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

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }



}

