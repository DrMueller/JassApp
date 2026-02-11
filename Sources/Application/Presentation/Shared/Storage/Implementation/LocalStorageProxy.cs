using JassApp.Common.Extensions;
using JassApp.Common.LanguageExtensions.Types.Maybes;
using JassApp.Presentation.Infrastructure.Disposing;
using JassApp.Presentation.Infrastructure.JavaScript.Services;
using Microsoft.JSInterop;

namespace JassApp.Presentation.Shared.Storage.Implementation
{
    public sealed class LocalStorageProxy(
        IJavaScriptLocator jsLocator,
        IJSRuntime jsRuntime) : ILocalStorageProxy
    {
        private const string GetItemMethod = "getItem";
        private const string JavaScriptStoragePath = "./js/localstorage.js";
        private const string SetItemMethod = "setItem";

        private Lazy<IJSObjectReference> _accessorJsRef = new();

        public async ValueTask DisposeAsync()
        {
            await ComponentDisposeHandler.HandleDisposeAsync(async () =>
            {
                if (_accessorJsRef.IsValueCreated)
                {
                    await _accessorJsRef.Value.DisposeAsync();
                }
            });
        }

        public async Task<Maybe<string>> GetStringAsync(string key)
        {
            await WaitForReferenceAsync();
            return await _accessorJsRef.Value
                .InvokeAsync<string?>(GetItemMethod, key)
                .MapAsync(MaybeFactory.CreateFromNullable);
        }

        public async Task SetItemAsync(string key, string value)
        {
            await WaitForReferenceAsync();
            await _accessorJsRef.Value.InvokeVoidAsync(SetItemMethod, key, value);
        }

        private async Task WaitForReferenceAsync()
        {
            if (_accessorJsRef.IsValueCreated is false)
            {
                var jsPath = await jsLocator.LocateAbsoluteJsFilePathAsync(JavaScriptStoragePath);
                _accessorJsRef = new Lazy<IJSObjectReference>(await jsRuntime.InvokeAsync<IJSObjectReference>("import", jsPath));
            }
        }
    }
}