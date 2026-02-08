using JassApp.Presentation.Infrastructure.JavaScript.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace JassApp.Presentation.Areas.Test.Components
{
    public partial class TestVoiceInfosPage
    {
        private const string Path = "/test/voiceinfos";
        private DotNetObjectReference<TestVoiceInfosPage>? _dotNetRef;

        private bool _isInitialized;
        private readonly List<string> _log = new();

        private IJSObjectReference? _module;
        private string _preferredLang = "de-CH";
        private string? _preferredVoiceName;
        private SynthState? _state;
        private string _text = "Hallo zäme! Das ist ein Test.";

        private List<VoiceInfo> _voices = new();

        [Inject]
        public required IJavaScriptLocator JsLocator { get; set; }

        [Inject]
        public required IJSRuntime JsRuntime { get; set; }

        public async ValueTask DisposeAsync()
        {
            try
            {
                _dotNetRef?.Dispose();
                if (_module is not null)
                {
                    await _module.DisposeAsync();
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }

        [JSInvokable]
        public Task OnVoiceEventAsync(VoiceEvent e)
        {
            // keep log short but useful
            var extra =
                e.Error is not null ? $" error={e.Error}" :
                e.Name is not null ? $" boundary={e.Name}@{e.CharIndex}" :
                e.Message is not null ? $" msg={e.Message}" :
                "";

            if (e.State is not null)
            {
                extra += $" (speaking={e.State.Speaking}, pending={e.State.Pending}, paused={e.State.Paused})";
            }

            if (e.Voice is not null)
            {
                extra += $" voice={e.Voice}";
            }

            if (e.Lang is not null)
            {
                extra += $" lang={e.Lang}";
            }

            _log.Insert(0, $"{e.Ts} | {e.Type}{extra}");

            // avoid unbounded growth
            if (_log.Count > 300)
            {
                _log.RemoveRange(300, _log.Count - 300);
            }

            StateHasChanged();
            return Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await AssureJavaScriptModuleAsync();
            }
        }

        private async Task AssureJavaScriptModuleAsync()
        {
            var jsFilePath = await JsLocator.LocateJsFilePathAsync(this);
            _module ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", jsFilePath);

            _dotNetRef = DotNetObjectReference.Create(this);

            await _module.InvokeVoidAsync("init", _dotNetRef);
            _isInitialized = true;

            await RefreshVoicesAsync();
            await RefreshStateAsync();

            StateHasChanged();
        }

        private async Task CancelAsync()
        {
            if (_module is null)
            {
                return;
            }

            await _module.InvokeVoidAsync("cancel");
            await RefreshStateAsync();
        }

        private void ClearLog()
        {
            _log.Clear();
        }

        private async Task PrimeAsync()
        {
            if (_module is null)
            {
                return;
            }

            await _module.InvokeVoidAsync("prime");
            await RefreshStateAsync();
        }

        private async Task RefreshStateAsync()
        {
            if (_module is null)
            {
                return;
            }

            _state = await _module.InvokeAsync<SynthState>("state");
        }

        private async Task RefreshVoicesAsync()
        {
            if (_module is null)
            {
                return;
            }

            _voices = (await _module.InvokeAsync<VoiceInfo[]>("getVoices")).ToList();
        }

        private async Task SpeakAsync()
        {
            if (_module is null)
            {
                return;
            }

            // Important on iOS: call speak as early as possible in the click handler.
            await _module.InvokeVoidAsync("speak", _text, _preferredLang, _preferredVoiceName);
            await RefreshStateAsync();
        }

        public record VoiceInfo(string Name, string Lang, bool Default, bool LocalService);

        public record SynthState(bool Supported, bool Speaking, bool Pending, bool Paused);

        public record VoiceEvent(
            string Type,
            string Ts,
            string? Error,
            string? Message,
            string? Name,
            int? CharIndex,
            string? Voice,
            string? Lang,
            SynthState? State
        );
    }
}