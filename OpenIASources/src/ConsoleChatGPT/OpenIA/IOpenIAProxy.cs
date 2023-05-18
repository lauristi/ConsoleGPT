using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace ConsoleChatGPT.OpenIA;

public interface IOpenIAProxy
{
    Task<ChatCompletionMessage[]> SendChatMessage(string message);

    void SetSystemMessage(string systemMessage);
}
