using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

namespace StarterKit.SemanticKernelService.Services;

internal class AzureOAIChatService : IChatService
{
    private readonly AzureOpenAIChatCompletionService _chatCompletionService;

    public AzureOAIChatService(AzureOpenAIChatCompletionService chatCompletionService)
    {
        _chatCompletionService = chatCompletionService;
    }

    public async Task<ChatMessageContent?> GetResponseAsync(ChatHistory history)
    {
        var response = await _chatCompletionService.GetChatMessageContentAsync(history);
        return response;
    }

    public async IAsyncEnumerable<StreamingChatMessageContent?> GetStreamingResponseAsync(ChatHistory history)
    {
        var response = _chatCompletionService.GetStreamingChatMessageContentsAsync(history);

        await foreach (var chunk in response)
        {
            yield return chunk;
        }
    }
}
