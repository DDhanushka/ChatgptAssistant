using ChatgptAssistant;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("======== Chatgpt Assistant ========");

        AppSettings appSettings = Utils.GetAppSettings();


        while (true)
        {
            DisplayMenu(appSettings);

            // Get user input
            Console.Write("Enter your choice (1-4, or 'x' to exit): ");
            string? userInput = Console.ReadLine();

            // Check if the user wants to exit
            if (userInput?.ToLower() == "x")
            {
                break;
            }

            // Process user input
            ProcessUserInput(userInput);
        }
    }
    private static void DisplayMenu(AppSettings appSettings)
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Chat Mode");
        Console.WriteLine("2. Run Custsom Prompt");
        Console.WriteLine("\nX. Exit");
        Console.WriteLine();

        Console.WriteLine("Prompt Directory:\t" + appSettings.PromptDirectory);
        Console.WriteLine("Result Directory:\t" + appSettings.ResultDirectory);
        Console.WriteLine("ChatGPT Model:\t\t" + appSettings.OpenAIModelID);
        Console.WriteLine();
    }

    private static void ProcessUserInput(string? userInput)
    {
        switch (userInput)
        {
            case "1":
                Console.WriteLine("You selected: Chat Mode");
                break;
            case "2":
                Console.WriteLine("You selected: Custom Prompt");
                break;
            default:
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 4, or 'X' to exit.");
                break;
        }

        Console.WriteLine();
    }
}