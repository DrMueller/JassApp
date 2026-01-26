using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Infrastructure.JavaScript.Services.Implementation
{
    [UsedImplicitly]
    public class JavaScriptLocator : IJavaScriptLocator
    {
        public Task<string> LocateAbsoluteJsFilePathAsync(string absolutePath)
        {
            return Task.FromResult(absolutePath);
        }

        public Task<string> LocateJsFilePathAsync<T>() where T : ComponentBase
        {
            var type = typeof(T);

            var assemblyFullName = type.Assembly.FullName;
            var assemblyName = type.Assembly.FullName!.Substring(0, assemblyFullName!.IndexOf(','));
            var relativeNamespace = type.FullName!.Replace(assemblyName, string.Empty);

            if (type.IsGenericType)
            {
                relativeNamespace = relativeNamespace.Substring(0, relativeNamespace.IndexOf('`'));
            }

            var path = relativeNamespace.Replace(".", "/");
            path += ".razor.js";
            return Task.FromResult(path);
        }
    }
}