using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace webAPI_Demo._01 {

   
    public class FilterConfig {
        /// <summary>
        /// MVC Filter 註冊
        /// </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());            
        }

        /// <summary>
        /// webAPI Filter 註冊
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterWebApiFilters(HttpFilterCollection filters) {
            //filters.Add(new Filters.RequireHttpsAttribute());  // 強制SSL
        }
    }
}
