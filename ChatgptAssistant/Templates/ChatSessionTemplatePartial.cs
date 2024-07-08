namespace ChatgptAssistant.Templates
{
    public partial class ChatSessionTemplate
    {
        public Dictionary<Guid, ChatBubble>? ChatHistory { get; set; }
        public string? Title { get; set; }
    }

    public class ChatBubble
    {
        public string? Role { get; set; }
        public string? Message { get; set; }
    }
}
