using System.Web;
using System.Web.Mvc;

namespace MvcAuction
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //adds a global filter attribute that allows me to restrict access by callers to methods 
            filters.Add(new AuthorizeAttribute());
        }
    }
}