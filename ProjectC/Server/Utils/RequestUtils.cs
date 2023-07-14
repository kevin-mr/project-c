namespace ProjectC.Server.Utils
{
    public class RequestUtils
    {
        public static string BuildRegexPath(string path)
        {
            return path.Contains("{number}") ? path.Replace("{number}", "[0-9]+") : path;
        }
    }
}
