using MudBlazor;
using ProjectC.Shared.Models;

namespace ProjectC.Client.Utils
{
    public static class RequestUtils
    {
        public static Color GetMethodColor(this RequestRuleMethodDto method)
        {
            switch (method)
            {
                case RequestRuleMethodDto.GET:
                    return Color.Success;
                case RequestRuleMethodDto.POST:
                    return Color.Warning;
                case RequestRuleMethodDto.PUT:
                    return Color.Info;
                case RequestRuleMethodDto.DELETE:
                    return Color.Error;
                default:
                    return Color.Default;
            }
        }

        public static Color GetMethodColor(this RequestEventDto requestEvent)
        {
            switch (requestEvent.Method)
            {
                case "GET":
                    return Color.Success;
                case "POST":
                    return Color.Warning;
                case "PUT":
                    return Color.Info;
                case "DELETE":
                    return Color.Error;
                default:
                    return Color.Default;
            }
        }
    }
}
