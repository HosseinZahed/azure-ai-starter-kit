using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace StarterKit.SemanticKernelService.Services;

public class AzureOpenAIService(
    [FromKeyedServices(Constants.StarterKitKernel)] Kernel starterKitKernel,
    IChatCompletionService chatCompletionService) : IAzureOpenAIService
{
    private readonly Kernel _starterKitKernel = starterKitKernel;
    private readonly IChatCompletionService _chatCompletionService = chatCompletionService;

    public async Task<string?> GetChatResponseAsync(ChatHistory chatHistory,
        CancellationToken cancellationToken)
    {
        var response = await _chatCompletionService.GetChatMessageContentAsync(
            chatHistory: chatHistory,
            executionSettings: OpenAIPromptExecutionSettings,
            kernel: _starterKitKernel,
            cancellationToken: cancellationToken);

        return response?.Content;
    }

    public async IAsyncEnumerable<string?> GetChatStreamingResponseAsync(
        ChatHistory chatHistory, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var response = _chatCompletionService.GetStreamingChatMessageContentsAsync(
            chatHistory: chatHistory,
            executionSettings: OpenAIPromptExecutionSettings,
            kernel: _starterKitKernel,
            cancellationToken: cancellationToken);

        await foreach (var chunk in response.WithCancellation(cancellationToken))
        {
            yield return chunk?.Content;
        }
    }

    private OpenAIPromptExecutionSettings OpenAIPromptExecutionSettings => new()
    {
        FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
    };
}
