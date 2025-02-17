using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using StarterKit.SemanticKernelService.Options;

namespace StarterKit.SemanticKernelService;

public static class Extensions
{
    public static TBuilder AddSemanticKernelService<TBuilder>(this TBuilder builder, IConfiguration? configuration) where TBuilder : IHostApplicationBuilder
    {
        // Azure OpenAI configuration
        builder.Services.AddOptions<AzureOpenAIOptions>()
            .Bind(builder.Configuration.GetSection(nameof(AzureOpenAIOptions.SectionName)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Ollama configuration
        builder.Services.AddOptions<OllamaOptions>()
            .Bind(builder.Configuration.GetSection(nameof(OllamaOptions.SectionName)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Retrieve the option values
        var serviceProvider = builder.Services.BuildServiceProvider();
        var azureOpenAIOptions = serviceProvider.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;
        var ollamaOptions = serviceProvider.GetRequiredService<IOptions<OllamaOptions>>().Value;

        // Register Azure OpenAI chat completion service
        builder.Services.AddAzureOpenAIChatCompletion(
            deploymentName: azureOpenAIOptions.ChatDeploymentName,
            endpoint: azureOpenAIOptions.Endpoint,
            apiKey: azureOpenAIOptions.ApiKey
        );

        // Register Ollama chat completion service
#pragma warning disable SKEXP0070
        builder.Services.AddOllamaChatCompletion(
            modelId: ollamaOptions.ModelName,
            endpoint: new Uri(ollamaOptions.Endpoint)
         );
        return builder;
    }
}
