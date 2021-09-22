using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KELPortableFileServer.Server.ActionResults
{
    public class StatusCodeOnlyResult : ActionResult
    {
        protected int StatusCode;

        public StatusCodeOnlyResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            return base.ExecuteResultAsync(context);
        }
    }
}
