// Ignore Spelling: Api

using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.SemanticKernel.ChatCompletion;

namespace StarterKit.Web;

public class ApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string?> GetAzureOpenAIChatResponseAsync(ChatHistory chatHistory,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/chat/aoai/non-stream", chatHistory, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        }
        else
        {
            throw new Exception($"Error in response!");
        }
    }

    public async IAsyncEnumerable<string?> GetAzureOpenAIChatStreamingResponseAsync(
        ChatHistory chatHistory, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/chat/aoai/stream")
        {
            Content = JsonContent.Create(chatHistory),
            Headers = { Accept = { new MediaTypeWithQualityHeaderValue("text/event-stream") } }
        };
        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var chunkResponse = await reader.ReadLineAsync(cancellationToken);
                yield return chunkResponse;
                await Task.Delay(100, cancellationToken);
            }
        }
        else
        {
            throw new Exception($"Error in response!");
        }
    }
}
