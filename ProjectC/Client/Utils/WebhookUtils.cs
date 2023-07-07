using MudBlazor;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Utils
{
    public static class WebhookUtils
    {
        public static Color GetMethodColor(this WebhookRuleMethodDto method)
        {
            switch (method)
            {
                case WebhookRuleMethodDto.POST:
                    return Color.Warning;
                case WebhookRuleMethodDto.PUT:
                    return Color.Info;
                default:
                    return Color.Default;
            }
        }
    }
}
