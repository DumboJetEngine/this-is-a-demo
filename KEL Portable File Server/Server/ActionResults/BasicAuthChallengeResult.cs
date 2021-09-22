using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KELPortableFileServer.Server.ActionResults
{
    public class BasicAuthChallengeResult : StatusCodeOnlyResult
    {
        private string _realm;

        public BasicAuthChallengeResult(string realm = "") : base(StatusCodes.Status401Unauthorized)
        {
            _realm = realm;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            context.HttpContext.Response.Headers.Add("WWW-Authenticate", $"{"Basic"} Realm=\"{_realm}\"");
            return base.ExecuteResultAsync(context);
        }
    }
}
