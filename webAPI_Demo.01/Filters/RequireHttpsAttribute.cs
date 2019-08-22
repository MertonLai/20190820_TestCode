using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace webAPI_Demo._01.Filters {

    /// <summary>
    /// 強制HTTPS
    /// </summary>
    public class RequireHttpsAttribute : AuthorizationFilterAttribute {
        public override void OnAuthorization(HttpActionContext actionContext) {

            // 強制使用SSL（HTTPS）
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps) {
                actionContext.Response =
                new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden) {
                    ReasonPhrase = "HTTPS  Required"
                };
            } else {
                base.OnAuthorization(actionContext);
            }
        }
    }
}