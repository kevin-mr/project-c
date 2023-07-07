using MudBlazor;

namespace ProjectC.Client.Utils
{
    public static class DialogUtils
    {
        public static DialogOptions DefaultDialogOptions =
            new()
            {
                CloseOnEscapeKey = true,
                CloseButton = true,
                FullWidth = true,
                MaxWidth = MaxWidth.Small,
            };
    }
}
