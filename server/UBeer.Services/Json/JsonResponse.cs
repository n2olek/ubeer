using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UBeer.Models;

namespace UBeer.Services
{
    public class JsonResponse : IJsonResponse
    {

        #region Private Variables

        private JSONTemplate objTemlpate;

        #endregion

        #region Status 1xx Informational
        public JSONTemplate Continue100(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 100;
            objTemlpate.data = data;
            objTemlpate.message = "Continue.";
            return objTemlpate;
        }
        public JSONTemplate SwitchingProtocols101(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 101;
            objTemlpate.data = data;
            objTemlpate.message = "Switching Protocols.";
            return objTemlpate;
        }
        public JSONTemplate Processing102(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 102;
            objTemlpate.data = data;
            objTemlpate.message = "Processing.";
            return objTemlpate;
        }

        #endregion

        #region Status 2xx Success
        public JSONTemplate OK200(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 200;
            objTemlpate.data = data;
            objTemlpate.message = "OK.";
            return objTemlpate;
        }
        public JSONTemplate Created201(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 201;
            objTemlpate.data = data;
            objTemlpate.message = "Created.";
            return objTemlpate;
        }
        public JSONTemplate Accepted202(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 202;
            objTemlpate.data = data;
            objTemlpate.message = "Accepted.";
            return objTemlpate;
        }
        public JSONTemplate NonAuthoritativeInformation203(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 203;
            objTemlpate.data = data;
            objTemlpate.message = "Non-Authoritative Information.";
            return objTemlpate;
        }
        public JSONTemplate NoContent204(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 204;
            objTemlpate.data = data;
            objTemlpate.message = "No Content.";
            return objTemlpate;
        }
        public JSONTemplate ResetContent205(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 205;
            objTemlpate.data = data;
            objTemlpate.message = "Reset Content.";
            return objTemlpate;
        }
        public JSONTemplate PartialContent206(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 206;
            objTemlpate.data = data;
            objTemlpate.message = "Partial Content.";
            return objTemlpate;
        }
        public JSONTemplate MultiStatus207(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 207;
            objTemlpate.data = data;
            objTemlpate.message = "Multi-Status.";
            return objTemlpate;
        }
        public JSONTemplate AlreadyReported208(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 208;
            objTemlpate.data = data;
            objTemlpate.message = "Already Reported.";
            return objTemlpate;
        }

        #endregion

        #region Status 3xx Redirection

        public JSONTemplate MultipleChoices300(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 300;
            objTemlpate.data = data;
            objTemlpate.message = "Multiple Choices.";
            return objTemlpate;
        }
        public JSONTemplate MovedPermanently301(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 301;
            objTemlpate.data = data;
            objTemlpate.message = "Moved Permanently.";
            return objTemlpate;
        }
        public JSONTemplate Found302(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 302;
            objTemlpate.data = data;
            objTemlpate.message = "Found.";
            return objTemlpate;
        }
        public JSONTemplate SeeOther303(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 303;
            objTemlpate.data = data;
            objTemlpate.message = "See Other.";
            return objTemlpate;
        }
        public JSONTemplate NotModified304(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 304;
            objTemlpate.data = data;
            objTemlpate.message = "Not Modified.";
            return objTemlpate;
        }
        public JSONTemplate UseProxy305(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 305;
            objTemlpate.data = data;
            objTemlpate.message = "Use Proxy.";
            return objTemlpate;
        }
        public JSONTemplate SwitchProxy306(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 306;
            objTemlpate.data = data;
            objTemlpate.message = "Switch Proxy.";
            return objTemlpate;
        }
        public JSONTemplate TemporaryRedirect307(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 307;
            objTemlpate.data = data;
            objTemlpate.message = "Temporary Redirect.";
            return objTemlpate;
        }
        public JSONTemplate PermanentRedirect308(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 308;
            objTemlpate.data = data;
            objTemlpate.message = "Permanent Redirect.";
            return objTemlpate;
        }

        #endregion

        #region Status 4xx Client Error

        public JSONTemplate BadRequest400(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 400;
            objTemlpate.data = data;
            objTemlpate.message = "Bad Request.";
            return objTemlpate;
        }

        public JSONTemplate Unauthorized401(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 401;
            objTemlpate.data = data;
            objTemlpate.message = "Unauthorized.";
            return objTemlpate;
        }

        public JSONTemplate PaymentRequired402(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 402;
            objTemlpate.data = data;
            objTemlpate.message = "Payment Required.";
            return objTemlpate;
        }

        public JSONTemplate Forbidden403(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 403;
            objTemlpate.data = data;
            objTemlpate.message = "Forbidden.";
            return objTemlpate;
        }

        public JSONTemplate NotFound404(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 404;
            objTemlpate.data = data;
            objTemlpate.message = "Not Found.";
            return objTemlpate;
        }

        public JSONTemplate MethodNotAllowed405(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 405;
            objTemlpate.data = data;
            objTemlpate.message = "Method Not Allowed.";
            return objTemlpate;
        }

        public JSONTemplate NotAcceptable406(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 406;
            objTemlpate.data = data;
            objTemlpate.message = "Not Acceptable.";
            return objTemlpate;
        }

        public JSONTemplate ProxyAuthenticationRequired407(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 407;
            objTemlpate.data = data;
            objTemlpate.message = "Proxy Authentication Required.";
            return objTemlpate;
        }

        public JSONTemplate RequestTimeout408(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 408;
            objTemlpate.data = data;
            objTemlpate.message = "Request Timeout.";
            return objTemlpate;
        }

        public JSONTemplate Conflict409(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 409;
            objTemlpate.data = data;
            objTemlpate.message = "Conflict.";
            return objTemlpate;
        }

        public JSONTemplate Gone410(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 410;
            objTemlpate.data = data;
            objTemlpate.message = "Gone.";
            return objTemlpate;
        }

        public JSONTemplate LengthRequired411(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 411;
            objTemlpate.data = data;
            objTemlpate.message = "Length Required.";
            return objTemlpate;
        }

        public JSONTemplate PreconditionFailed412(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 412;
            objTemlpate.data = data;
            objTemlpate.message = "Precondition Failed.";
            return objTemlpate;
        }

        public JSONTemplate RequestEntityTooLarge413(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 413;
            objTemlpate.data = data;
            objTemlpate.message = "Request Entity Too Large.";
            return objTemlpate;
        }

        public JSONTemplate RequestURITooLong414(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 414;
            objTemlpate.data = data;
            objTemlpate.message = "Request-URI Too Long.";
            return objTemlpate;
        }

        public JSONTemplate UnsupportedMediaType415(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 415;
            objTemlpate.data = data;
            objTemlpate.message = "Unsupported Media Type.";
            return objTemlpate;
        }

        public JSONTemplate RequestedRangeNotSatisfiable416(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 416;
            objTemlpate.data = data;
            objTemlpate.message = "Requested Range Not Satisfiable.";
            return objTemlpate;
        }

        public JSONTemplate ExpectationFailed417(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 417;
            objTemlpate.data = data;
            objTemlpate.message = "Expectation Failed.";
            return objTemlpate;
        }

        public JSONTemplate Iamateapot418(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 418;
            objTemlpate.data = data;
            objTemlpate.message = "I'm a teapot.";
            return objTemlpate;
        }

        public JSONTemplate AuthenticationTimeout419(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 419;
            objTemlpate.data = data;
            objTemlpate.message = "Authentication Timeout.";
            return objTemlpate;
        }

        public JSONTemplate MethodFailure420(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 420;
            objTemlpate.data = data;
            objTemlpate.message = "Method Failure.";
            return objTemlpate;
        }

        public JSONTemplate EnhanceYourCalm421(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 421;
            objTemlpate.data = data;
            objTemlpate.message = "Enhance Your Calm.";
            return objTemlpate;
        }

        public JSONTemplate UnprocessableEntity422(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 422;
            objTemlpate.data = data;
            objTemlpate.message = "Unprocessable Entity.";
            return objTemlpate;
        }

        public JSONTemplate Locked423(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 423;
            objTemlpate.data = data;
            objTemlpate.message = "Locked.";
            return objTemlpate;
        }

        public JSONTemplate FailedDependency424(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 424;
            objTemlpate.data = data;
            objTemlpate.message = "Failed Dependency.";
            return objTemlpate;
        }

        public JSONTemplate UpgradeRequired426(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 426;
            objTemlpate.data = data;
            objTemlpate.message = "Upgrade Required.";
            return objTemlpate;
        }

        public JSONTemplate PreconditionRequired428(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 428;
            objTemlpate.data = data;
            objTemlpate.message = "Precondition Required.";
            return objTemlpate;
        }

        public JSONTemplate TooManyRequests429(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 429;
            objTemlpate.data = data;
            objTemlpate.message = "Too Many Requests.";
            return objTemlpate;
        }

        public JSONTemplate RequestHeaderFieldsTooLarge431(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 431;
            objTemlpate.data = data;
            objTemlpate.message = "Request Header Fields Too Large.";
            return objTemlpate;
        }

        public JSONTemplate LoginTimeout440(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 440;
            objTemlpate.data = data;
            objTemlpate.message = "Login Timeout.";
            return objTemlpate;
        }

        public JSONTemplate NoResponse444(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 444;
            objTemlpate.data = data;
            objTemlpate.message = "No Response.";
            return objTemlpate;
        }

        public JSONTemplate RetryWith449(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 449;
            objTemlpate.data = data;
            objTemlpate.message = "Retry With.";
            return objTemlpate;
        }

        public JSONTemplate BlockedbyWindowsParentalControls450(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 450;
            objTemlpate.data = data;
            objTemlpate.message = "Blocked by Windows Parental Controls.";
            return objTemlpate;
        }

        public JSONTemplate UnavailableForLegalReasons451(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 451;
            objTemlpate.data = data;
            objTemlpate.message = "Unavailable For Legal Reasons.";
            return objTemlpate;
        }

        public JSONTemplate Redirect451(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 451;
            objTemlpate.data = data;
            objTemlpate.message = "Redirect.";
            return objTemlpate;
        }

        public JSONTemplate RequestHeaderTooLarge494(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 494;
            objTemlpate.data = data;
            objTemlpate.message = "Request Header Too Large.";
            return objTemlpate;
        }

        public JSONTemplate CertError495(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 495;
            objTemlpate.data = data;
            objTemlpate.message = "Cert Error.";
            return objTemlpate;
        }

        public JSONTemplate NoCert496(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 496;
            objTemlpate.data = data;
            objTemlpate.message = "No Cert.";
            return objTemlpate;
        }

        public JSONTemplate HTTPtoHTTPS497(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 497;
            objTemlpate.data = data;
            objTemlpate.message = "HTTP to HTTPS.";
            return objTemlpate;
        }

        public JSONTemplate Tokenexpired_invalid498(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 498;
            objTemlpate.data = data;
            objTemlpate.message = "Token expired/invalid.";
            return objTemlpate;
        }

        public JSONTemplate ClientClosedRequest499(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 499;
            objTemlpate.data = data;
            objTemlpate.message = "Client Closed Request.";
            return objTemlpate;
        }

        public JSONTemplate Tokenrequired499(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 499;
            objTemlpate.data = data;
            objTemlpate.message = "Token required.";
            return objTemlpate;
        }

        #endregion

        #region Status 5xx Server Error

        public JSONTemplate InternalServerError500(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 500;
            objTemlpate.data = data;
            objTemlpate.message = "Internal Server Error.";
            return objTemlpate;
        }

        public JSONTemplate NotImplemented501(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 501;
            objTemlpate.data = data;
            objTemlpate.message = "Not Implemented.";
            return objTemlpate;
        }

        public JSONTemplate BadGateway502(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 502;
            objTemlpate.data = data;
            objTemlpate.message = "Bad Gateway.";
            return objTemlpate;
        }

        public JSONTemplate ServiceUnavailable503(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 503;
            objTemlpate.data = data;
            objTemlpate.message = "Service Unavailable.";
            return objTemlpate;
        }

        public JSONTemplate GatewayTimeout504(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 504;
            objTemlpate.data = data;
            objTemlpate.message = "Gateway Timeout.";
            return objTemlpate;
        }

        public JSONTemplate HTTPVersionNotSupported505(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 505;
            objTemlpate.data = data;
            objTemlpate.message = "HTTP Version Not Supported.";
            return objTemlpate;
        }

        public JSONTemplate VariantAlsoNegotiates506(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 506;
            objTemlpate.data = data;
            objTemlpate.message = "Variant Also Negotiates.";
            return objTemlpate;
        }

        public JSONTemplate InsufficientStorage507(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 507;
            objTemlpate.data = data;
            objTemlpate.message = "Insufficient Storage.";
            return objTemlpate;
        }

        public JSONTemplate LoopDetected508(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 508;
            objTemlpate.data = data;
            objTemlpate.message = "Loop Detected.";
            return objTemlpate;
        }

        public JSONTemplate BandwidthLimitExceeded509(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 509;
            objTemlpate.data = data;
            objTemlpate.message = "Bandwidth Limit Exceeded.";
            return objTemlpate;
        }

        public JSONTemplate NotExtended510(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 510;
            objTemlpate.data = data;
            objTemlpate.message = "Not Extended.";
            return objTemlpate;
        }

        public JSONTemplate NetworkAuthenticationRequired511(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 511;
            objTemlpate.data = data;
            objTemlpate.message = "Network Authentication Required.";
            return objTemlpate;
        }

        public JSONTemplate OriginError520(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 520;
            objTemlpate.data = data;
            objTemlpate.message = "Origin Error.";
            return objTemlpate;
        }

        public JSONTemplate WebServerIsDown521(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 521;
            objTemlpate.data = data;
            objTemlpate.message = "Web server is down.";
            return objTemlpate;
        }

        public JSONTemplate ConnectionTimedOut522(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 522;
            objTemlpate.data = data;
            objTemlpate.message = "Connection timed out.";
            return objTemlpate;
        }

        public JSONTemplate ProxyDeclinedRequest523(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 523;
            objTemlpate.data = data;
            objTemlpate.message = "Proxy Declined Request.";
            return objTemlpate;
        }

        public JSONTemplate ATimeoutOcurred524(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 524;
            objTemlpate.data = data;
            objTemlpate.message = "A Timeout occurred.";
            return objTemlpate;
        }

        public JSONTemplate NetworkReadTimeoutError598(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 598;
            objTemlpate.data = data;
            objTemlpate.message = "Network read Timeout Error.";
            return objTemlpate;
        }

        public JSONTemplate NetworkConnectTimeoutError599(object data)
        {
            objTemlpate = new JSONTemplate();
            objTemlpate.status = 599;
            objTemlpate.data = data;
            objTemlpate.message = "Network connect Timeout Error.";
            return objTemlpate;
        }

        #endregion
    }
}
