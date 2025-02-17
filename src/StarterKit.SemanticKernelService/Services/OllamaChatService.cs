// Ignore Spelling: Ollama

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace StarterKit.SemanticKernelService.Services;

internal class OllamaChatService : IChatService
{
    private readonly OllamaChatService _chatCompletionService;

    public OllamaChatService(OllamaChatService chatCompletionService)
    {
        _chatCompletionService = chatCompletionService;
    }

    public async Task<ChatMessageContent?> GetResponseAsync(ChatHistory history)
    {
        var response = await _chatCompletionService.GetResponseAsync(history);
        return response;
    }

    public async IAsyncEnumerable<StreamingChatMessageContent?> GetStreamingResponseAsync(ChatHistory history)
    {
        var response = _chatCompletionService.GetStreamingResponseAsync(history);

        await foreach (var chunk in response)
        {
            yield return chunk;
        }
    }
}
