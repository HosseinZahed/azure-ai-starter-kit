using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace StarterKit.SemanticKernelService.Services;

internal interface IChatService
{
    Task<ChatMessageContent?> GetResponseAsync(ChatHistory history);

    IAsyncEnumerable<StreamingChatMessageContent?> GetStreamingResponseAsync(ChatHistory history);
}
