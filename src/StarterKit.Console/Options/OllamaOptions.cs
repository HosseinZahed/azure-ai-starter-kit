// Ignore Spelling: Ollama

using System.ComponentModel.DataAnnotations;

namespace StarterKit.ConsoleApp.Options;

public sealed class OllamaOptions
{
    public const string SectionName = "Ollama";

    [Required]
    public string ModelName { get; set; } = string.Empty;

    [Required]
    public string Endpoint { get; set; } = string.Empty;
}
