using JetBrains.Annotations;

namespace JassApp.Presentation.Areas.Test.Components
{
    [UsedImplicitly]
    public partial class TestExceptionPage
    {
        private const string Path = "/test/exception";

        private static string ErrorPropertyWithException => throw new Exception("Test exception");
    }
}