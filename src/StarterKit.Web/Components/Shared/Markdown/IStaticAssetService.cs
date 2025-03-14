namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IStaticAssetService
{
    public Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
