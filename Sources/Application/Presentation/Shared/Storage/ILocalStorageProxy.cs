using JassApp.Common.LanguageExtensions.Types.Maybes;

namespace JassApp.Presentation.Shared.Storage
{
    public interface ILocalStorageProxy : IAsyncDisposable
    {
        Task<Maybe<string>> GetStringAsync(string key);
        Task SetItemAsync(string key, string value);
    }
}