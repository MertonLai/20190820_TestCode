using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Task01.Models;

namespace Task01.Filters {
    public class APIErrorExceptionAttribute : ExceptionFilterAttribute {
        public override void OnException(HttpActionExecutedContext actionExecutedContext) {

            actionExecutedContext.Response = new HttpResponseMessage() {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ObjectContent<APIErrorMessage>(new APIErrorMessage() {
                    Error_Code = 9999,
                    Error_Message = actionExecutedContext.Exception.InnerException.ToString()
                }, GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };

            base.OnException(actionExecutedContext);
        }
    }
}