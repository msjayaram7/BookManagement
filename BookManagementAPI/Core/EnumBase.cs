using System.ComponentModel.DataAnnotations;

namespace BookmanagementAPI.Core
{
    public enum ResponseStatusCode
    {
        [Display(Name = "Successful API call")]
        Ok = 200,

        [Display(Name = "Multiple statuses are available for the request.")]
        MultiStatus = 300,

        [Display(Name = "Validation error has occurred.")]
        BadRequest = 400,

        [Display(Name = "Invalid authorization credentials.")]
        Unauthorized = 401,

        [Display(Name = "The Credit in your account is not sufficient to transmit all requested records")]
        CreditsNotSufficient = 402,

        [Display(Name = "The request method (POST or GET) is not allowed on the requested resource")]
        MethodNotAllowed = 405,

        [Display(Name = "The resource you have specified cannot be found")]
        NotFound = 404,

        [Display(Name = "The Media Type you have specified is Unsupported")]
        UnsupportedMediaType = 415,

        [Display(Name = "The API rate limit for your account has exceeded. You can only send a maximum of @@RequestCount requests per minute. Please send the next request after 10 seconds.")]
        RequestLimitExceeded = 426,

        [Display(Name = "Some error occurred with this API call. Please contact system administrator")]
        InternalServerError = 500,

        [Display(Name = "API is currently unavailable – typically due to a scheduled outage – try again soon.")]
        NotAvailable = 503
    }
}
