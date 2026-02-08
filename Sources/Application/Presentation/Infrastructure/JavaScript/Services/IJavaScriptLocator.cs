using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Infrastructure.JavaScript.Services
{
    public interface IJavaScriptLocator
    {
        Task<string> LocateJsFilePathAsync(ComponentBase component);

        Task<string> LocateAbsoluteJsFilePathAsync(string absolutePath);
    }
}