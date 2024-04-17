using System.Security.Principal;
using System;
using System.Web.Mvc;
using System.Web;
using SBM_POWER_BI.Classes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class JwtAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
{
    protected virtual bool AuthorizeCore(HttpContextBase httpContext)
    {
        JwtTokenGenerator JWT = new JwtTokenGenerator();
        if (httpContext == null)
        {
            throw new ArgumentNullException("httpContext");
        }
        IPrincipal user = httpContext.User;
        if (!JWT.IsAuth())
        {
            return false;
        }
        return true;
    }

    private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
    {
        validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
    }

    public virtual void OnAuthorization(AuthorizationContext filterContext)
    {
        if (filterContext == null)
        {
            throw new ArgumentNullException("filterContext");
        }
        if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
        {
            throw new ArgumentNullException("filterContext");
        }
        if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
        {
            if (AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetProxyMaxAge(new TimeSpan(0L));
                cache.AddValidationCallback(CacheValidateHandler, null);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }
    }

    protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new HttpUnauthorizedResult();
    }

    protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException("httpContext");
        }

        if (!AuthorizeCore(httpContext))
        {
            return HttpValidationStatus.IgnoreThisRequest;
        }

        return HttpValidationStatus.Valid;
    }
    public JwtAuthorizeAttribute()
    {

    }
}

