using JassApp.Common.InformationHandling;

namespace JassApp.Presentation.Areas.Test.Components
{
    public partial class TestInformationsPage
    {
        private const string Path = "/test/informations";
        private InformationEntries? Infos { get; set; }

        protected override void OnInitialized()
        {
            Infos = InformationEntries
                .CreateFromError("This is an error")
                .AddError("This is an error2")
                .AddWarning("This is a warning")
                .AddWarning("This is a warning2")
                .AddInformation("This is an info")
                .AddInformation("This is an info1");
        }
    }
}