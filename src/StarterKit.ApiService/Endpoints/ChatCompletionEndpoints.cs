using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.ChatCompletion;
using StarterKit.SemanticKernelService.Services;

namespace StarterKit.ApiService.Endpoints;

public static class ChatCompletionEndpoints
{
    public static void RegisterChatCompletionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/chat/aoai/non-stream",
            async ([FromServices] IAzureOpenAIService azureOpenAIService, [FromBody] ChatHistory chatHistory,
            CancellationToken cancellationToken) =>
            {
                var response = await azureOpenAIService.GetChatResponseAsync(chatHistory, cancellationToken);
                return response;
            }
        );

        app.MapPost("api/chat/aoai/stream",
            async (HttpContext context, [FromServices] IAzureOpenAIService chatService,
                [FromBody] ChatHistory chatHistory, CancellationToken cancellationToken, int chunkSize = 10) =>
            {
                context.Response.ContentType = "text/event-stream";
                await foreach (var chunkResponse in chatService.GetChatStreamingResponseAsync(chatHistory, cancellationToken))
                {
                    if (string.IsNullOrEmpty(chunkResponse))
                    {
                        continue;
                    }
                    Console.WriteLine(chunkResponse);
                    await context.Response.WriteAsync(chunkResponse);
                    await context.Response.Body.FlushAsync();
                }
            }
        );
    }
}
