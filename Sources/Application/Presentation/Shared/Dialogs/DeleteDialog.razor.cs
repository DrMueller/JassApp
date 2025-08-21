using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace JassApp.Presentation.Shared.Dialogs
{
    public partial class DeleteDialog
    {
        [CascadingParameter]
        public required IMudDialogInstance MudDialog { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private void Submit()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}