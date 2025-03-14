using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace StarterKit.ConsoleApp;

internal class Worker(
    IHostApplicationLifetime hostApplicationLifetime,
    [FromKeyedServices("StarterKitKerner")] Kernel starterKitKernel) : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime = hostApplicationLifetime;
    private readonly Kernel _starterKitKernel = starterKitKernel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Get chat completion service
        var chatCompletionService = _starterKitKernel.GetRequiredService<IChatCompletionService>();

        // Enable auto function calling
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        // Create a history store the conversation
        var chatHistory = new ChatHistory();

        // Initiate a back-and-forth chat
        string? userInput;
        do
        {
            // Collect user input
            Console.Write("User > ");
            userInput = Console.ReadLine();

            // Add user input
            chatHistory.AddUserMessage(userInput);

            // Get the response from the AI
            var result = await chatCompletionService.GetChatMessageContentAsync(
                chatHistory: chatHistory,
                executionSettings: openAIPromptExecutionSettings,
                kernel: _starterKitKernel,
                cancellationToken: stoppingToken);

            // Print the results
            Console.WriteLine("Assistant > " + result);

            // Add the message from the agent to the chat history
            chatHistory.AddMessage(result.Role, result.Content ?? string.Empty);
        } while (userInput is not null);

        _hostApplicationLifetime.StopApplication();
    }
}
