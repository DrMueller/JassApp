using JassApp.Presentation.Infrastructure.Caching.Controllers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace JassApp.Presentation.Infrastructure.JavaScript.Services.Implementation
{
    [UsedImplicitly]
    public class JavaScriptLocator(ICachingService cachingService) : IJavaScriptLocator
    {
        public async Task<string> LocateAbsoluteJsFilePathAsync(string absolutePath)
        {
            return await AppendCacheSuffixAsync(absolutePath);
        }

        public async Task<string> LocateJsFilePathAsync<T>() where T : ComponentBase
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

            path = await AppendCacheSuffixAsync(path);

            return path;
        }

        private async Task<string> AppendCacheSuffixAsync(string path)
        {
            var cacheSuffix = await cachingService.LoadCachingSuffixAsync();

            return path + cacheSuffix;
        }
    }
}