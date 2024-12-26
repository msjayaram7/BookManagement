using System.Security.Cryptography;
using System.Text;

namespace BookManagement.Core
{
    static class APIGenerateAuthHeader
    {
        public static class NewAPIGenerateAuthHeader
        {
            private static readonly string _userToken = Utility.Utility.GetAppSettings("UserToken");
            private static readonly string apiPrivateKey = Utility.Utility.GetAppSettings("PrivateKey");
            private static readonly string apiPublicKey = Utility.Utility.GetAppSettings("PublicKey");
            #region Generate Headers with Hashed Signature


            public static string BuildAuthSignature(string methodType, string date, string userToken, string absolutePath)
            {
                string message = string.Empty;
                absolutePath = "/" + absolutePath;
                var uri = absolutePath;
                message = string.Join("\n", methodType, date, userToken, uri); //, parameterMessage
                return message;
            }

            #region Compute Hash

            private static string ComputeHash(string privateKey, string message)
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

            #endregion Compute Hash

            #region Generate Auth Header

            public static void NewGenerateAuthHeader(HttpClient client, string requestUri, string methodType)
            {
                client.DefaultRequestHeaders.Clear();

                if (!string.IsNullOrWhiteSpace(_userToken))
                {
                    string authorizeHeader = !string.IsNullOrWhiteSpace(_userToken) ? _userToken : string.Empty;
                    client.DefaultRequestHeaders.Add("UserToken", authorizeHeader);
                }
                var utcDate = DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                string utcDateString = string.Format("{0:U}", utcDate); //"Thursday, May 21, 2015 4:33:50 AM"
                client.DefaultRequestHeaders.Add("Timestamp", utcDateString);
                string authenticationHeader = apiPublicKey + ":" + ComputeHash(apiPrivateKey, BuildAuthSignature(methodType, utcDateString, _userToken, requestUri.ToLower()));
                client.DefaultRequestHeaders.Add("Authentication", authenticationHeader);

            }

            #endregion Generate Auth Header

            #endregion Generate Headers with Hashed Signature
        }

    }
}
