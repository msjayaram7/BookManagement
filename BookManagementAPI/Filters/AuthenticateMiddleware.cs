using System.Runtime.Caching;
using BookmanagementAPI.Core;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using BookManagementAPI.Utilities;

namespace BookmanagementAPI.Filters
{
    public class AuthenticateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private static readonly string _userToken = Utility.GetAppSettings("UserToken");
        private static readonly string apiPrivateKey = Utility.GetAppSettings("PrivateKey");
        private static readonly string apiPublicKey = Utility.GetAppSettings("PublicKey");
        public AuthenticateMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task Invoke(HttpContext httpContext)
        {
            bool isAuthenticated = false;
            bool isAuthorized = false;
            string hostIp = "-";

            #region SWT logics 

            isAuthenticated = IsAuthenticated(httpContext);
            isAuthorized = IsAuthorized(httpContext);
            #endregion
            if (isAuthenticated && isAuthorized)
            {

            }
            else
            {

                httpContext.Response.StatusCode = (int)ResponseStatusCode.Unauthorized;
                Task<HttpContext> responseStatus = Task<HttpContext>.Factory.StartNew(() => httpContext);
                return responseStatus;
            }
            return _next(httpContext);
        }

        bool IsAuthenticated(HttpContext httpContext)
        {

            var timeStampString = _httpContextAccessor.HttpContext.Request.Headers["Timestamp"];

            string authenticationString = _httpContextAccessor.HttpContext.Request.Headers["Authentication"];
            if (string.IsNullOrWhiteSpace(authenticationString))
                return false;

            var authenticationParts = authenticationString.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            if (authenticationParts.Length != 2)
                return false;

            var publicKey = authenticationParts[0];
            var signature = authenticationParts[1];

            if (!IsSignatureValidated(signature))
                return false;

            AddToMemoryCache(signature);


            string authorizationString = _httpContextAccessor.HttpContext.Request.Headers["UserToken"];

            var authorizationPart = authorizationString.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (authorizationPart.Length > 0 && !string.IsNullOrWhiteSpace(authorizationString))
            {
                if (_userToken == authorizationPart[0])
                {
                    var baseString = BuildBaseString(httpContext);
                    return IsAuthenticated(apiPrivateKey, baseString, signature);

                }
            }

            return false;
        }

        static bool IsSignatureValidated(string signature)
        {
            var memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(signature))
                return false;

            return true;
        }

        static void AddToMemoryCache(string signature)
        {
            var memoryCache = MemoryCache.Default;
            if (!memoryCache.Contains(signature))
            {
                var expiration = DateTimeOffset.UtcNow.AddMinutes(5);
                memoryCache.Add(signature, signature, expiration);
            }
        }


        string BuildBaseString(HttpContext actionContext)
        {
            string date = _httpContextAccessor.HttpContext.Request.Headers["Timestamp"];

            string userToken = "-";


            string authorizationString = _httpContextAccessor.HttpContext.Request.Headers["UserToken"];

            if (!string.IsNullOrWhiteSpace(authorizationString))
            {
                var authenticationParts = authorizationString.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (authenticationParts != null && authenticationParts.Length > 0)
                {
                    userToken = authenticationParts[0];
                }
            }
            string methodType = actionContext.Request.Method;

            string absolutePath;
            string parameters;
           
                absolutePath = _httpContextAccessor.HttpContext.Request.Path.ToString().ToLower();

            parameters = _httpContextAccessor.HttpContext.Request.QueryString != null ? _httpContextAccessor.HttpContext.Request.QueryString.ToString().ToLower() : "-";
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                absolutePath = absolutePath + parameters;
            }

            var uri = WebUtility.UrlDecode(absolutePath);

            string message = string.Join("\n", methodType, date, userToken, uri);//, parameterMessage
            return message;
        }

        bool IsAuthorized(HttpContext actionContext)
        {

            string authorizationString = _httpContextAccessor.HttpContext.Request.Headers["UserToken"];
            string authenticationHeaderName = _httpContextAccessor.HttpContext.Request.Headers["Authentication"];
            if (!string.IsNullOrWhiteSpace(authorizationString) && !string.IsNullOrWhiteSpace(authenticationHeaderName))
            {
                //To DO: Get Header Data and Authroize the User
                var authenticationParts = authorizationString.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                var authenticationHeaderparts = authenticationHeaderName.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (authenticationParts.Length > 0 && authenticationHeaderparts.Length > 0)
                {
                    string accessToken = authenticationParts[0];
                    string publicKey = authenticationHeaderparts[0];
                    if (accessToken.Equals(_userToken, StringComparison.OrdinalIgnoreCase) &&  publicKey.Equals(apiPublicKey, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        static bool IsAuthenticated(string privateKey, string message, string signature)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                return false;
            var verifiedHash = ComputeHash(privateKey, message);
            if (signature != null && signature.Equals(verifiedHash))
                return true;

            return false;
        }

        static string ComputeHash(string privateKey, string message)
        {
            var key = Encoding.UTF8.GetBytes(privateKey);
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }


        
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticateMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticateMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticateMiddleware>();
        }
    }
}
