using SBM_POWER_BI;
using System;
using System.Web;
using System.Web.Mvc;

namespace SBM_POWER_BI.Classes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class GroupAuthorizeAttribute : AuthorizeAttribute
    {
        public string GroupCookieName { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            JwtTokenGenerator JWT = new JwtTokenGenerator();
            var isAuthorized = JWT.IsAuth();
            if (isAuthorized)
            {
                var userGroups = COOKIES.GetCookies(GroupCookieName);
                if (userGroups != null)
                {
                    if (userGroups.ToUpper() == "SYSTEM-ADMIN" || userGroups.ToUpper() == "SUPER-ADMIN")
                    {
                        return true;
                    }
                }
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 403;
                httpContext.Response.TrySkipIisCustomErrors = true;
                httpContext.Response.Redirect("~/Error/Index", endResponse: true);
                return false;
            }
            else
            {
                return isAuthorized;
            }
        }
    }
}