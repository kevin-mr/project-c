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

        public static DialogOptions DefaultDialogOptionsWidth(MaxWidth maxWidth)
        {
            return new()
            {
                CloseOnEscapeKey = true,
                CloseButton = true,
                FullWidth = true,
                MaxWidth = maxWidth,
            };
        }
    }
}
