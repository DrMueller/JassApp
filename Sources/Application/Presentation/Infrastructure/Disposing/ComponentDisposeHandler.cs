using Microsoft.JSInterop;

namespace JassApp.Presentation.Infrastructure.Disposing
{
    internal static class ComponentDisposeHandler
    {
        // Actual way of disposing, see https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/?view=aspnetcore-5.0#dom-cleanup-tasks-during-component-disposal
        internal static async ValueTask HandleDisposeAsync(Func<ValueTask> func)
        {
            try
            {
                await func();
            }
            catch (JSDisconnectedException)
            {
            }
        }
    }
}