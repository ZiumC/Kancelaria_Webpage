﻿using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace GB_Webpage.Services
{
    public class LogFormatterService
    {
        /// <summary>
        /// This method allows to format request to string. For async methods
        /// it would be needed to use GetAsyncMethodName() to get caller name.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="methodName">Method name</param>
        /// <returns>Formatted string ready to save in log app.</returns>
        public static string FormatRequest(HttpContext context, string? methodName)
        {
            string result = string.Empty;

            string? clientIp = context.Connection.RemoteIpAddress?.ToString();
            var clientParameters = context.Request.Query.ToList();

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

            result = $"REQUEST\t\tMethod:{methodName}\tIp:{clientIp}\tQueryString:{quertyString}";

            return result;
        }

        /// <summary>
        /// This method allows to get async method name.
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns>Returns async caller name</returns>
        public static string GetAsyncMethodName([CallerMemberName] string methodName = "")
        {
            return methodName;
        }

        /// <summary>
        /// This method allows to format exception to string.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <returns>Formatted string ready to save in log app.</returns>
        public static string FormatException(Exception e) 
        {
            string result = string.Empty;

            var stackTrace = new StackTrace(e, true);
            var frame = stackTrace.GetFrame(0);

            result = $"EXCEPTION\t\tException:{e.Message}\tAt:{frame?.GetFileLineNumber()}";

            return result;
        }

        public static string FormatAction(string action, string? details) 
        {
            string result = string.Empty;

            result = $"ACTION\t\tAction:{action}\tDetails:{details}";

            return result;
        }

    }
}
