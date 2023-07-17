using MudBlazor;
using ProjectC.Client.Components;

namespace ProjectC.Client.Utils
{
    public static class DialogUtils
    {
        public static async Task<bool> ShowConfirmDialog(
            this IDialogService dialogService,
            string title,
            string content,
            string button,
            Color color
        )
        {
            var parameters = new DialogParameters
            {
                ["ContentText"] = content,
                ["ButtonText"] = button,
                ["Color"] = color
            };

            var dialog = await dialogService.ShowAsync<ConfirmDialogComponent>(
                title,
                parameters,
                DefaultDialogOptionsWidth(MaxWidth.ExtraSmall)
            );
            var result = await dialog.Result;
            return result is not null && (result.Data is not null && (bool)result.Data);
        }

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
