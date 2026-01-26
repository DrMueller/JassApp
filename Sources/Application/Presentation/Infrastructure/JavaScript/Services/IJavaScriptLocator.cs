using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Infrastructure.JavaScript.Services
{
    public interface IJavaScriptLocator
    {
        Task<string> LocateJsFilePathAsync<T>()
            where T : ComponentBase;

        Task<string> LocateAbsoluteJsFilePathAsync(string absolutePath);
    }
}