namespace GB_Webpage.Services
{
    public class LogBuilder
    {
        public static string BuildLogFrom(HttpContext _context, string? _methodName)
        {
            string result = string.Empty;

            string? clientIp = _context.Connection.RemoteIpAddress?.ToString();
            var clientParameters = _context.Request.Query.ToList();

            string quertyString = "?";
            foreach (var parameter in clientParameters)
            {
                quertyString += $"{parameter.Key}={parameter.Value}&";
            }
            if (quertyString.Length > 0)
            {
                quertyString = quertyString.Remove(quertyString.Length - 1);
            }
            else
            {
                quertyString = string.Empty;
            }

            result = $"Method:{_methodName}\tIP:{clientIp}\tQueryString:{quertyString}";

            return result;
        }
    }
}
