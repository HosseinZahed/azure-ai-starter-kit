using StarterKit.ApiService.Endpoints;

namespace StarterKit.ApiService;

public static class Extensions
{
    public static void RegisterAllEndpoints(this WebApplication app)
    {
        // Register endpoints
        app.RegisterChatCompletionEndpoints();
    }
}
