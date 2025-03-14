using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using StarterKit.ConsoleApp;
using StarterKit.ConsoleApp.ConsolePlugIns;
using StarterKit.ConsoleApp.Options;

var builder = Host.CreateApplicationBuilder(args);

// Add User Secrets
builder.Configuration.AddUserSecrets<Worker>();

// Azure OpenAI configuration
builder.Services.AddOptions<AzureOpenAIOptions>()
    .Bind(builder.Configuration.GetSection(nameof(AzureOpenAIOptions.SectionName)));

// Get Azure OpenAI configuration
var azureOpenAIOptions = builder.Configuration.GetSection(AzureOpenAIOptions.SectionName).Get<AzureOpenAIOptions>()
    ?? throw new InvalidOperationException($"Missing configuration section '{AzureOpenAIOptions.SectionName}'");

// Actual code to execute is found in Worker class
builder.Services.AddHostedService<Worker>();

// Add Azure OpenAI Chat Completion Service
builder.Services.AddSingleton<IChatCompletionService>(sp =>
{
    return new AzureOpenAIChatCompletionService(
        azureOpenAIOptions.ChatDeploymentName,
        azureOpenAIOptions.Endpoint,
        azureOpenAIOptions.ApiKey);
});

// Add Plug-ins
builder.Services.AddSingleton<CurrentDateTimePlugin>();

// Add Kernel
builder.Services.AddKeyedTransient<Kernel>("StarterKitKerner", (sp, key) =>
{
    // Add ConsolePlugIns
    KernelPluginCollection pluginCollection = [];
    pluginCollection.AddFromObject(sp.GetRequiredService<CurrentDateTimePlugin>());

    // Create the Kernel
    return new Kernel(sp, pluginCollection);
});

using var host = builder.Build();

await host.RunAsync();
