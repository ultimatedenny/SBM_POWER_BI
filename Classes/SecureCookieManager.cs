using System;
using System.Web;
using System.Configuration;

namespace SBM_POWER_BI
{
    public class COOKIES
    {
        public static string GetWinID()
        {
            return GetCookies("WINDOWS_ID");
        }
        public static string GetName()
        {
            return GetCookies("NAME");
        }
        public static string GetEmailGroup()
        {
            return GetCookies("EMAIL");
        }
        public static string GetCookies(string Key)
        {
            string SITENAME = ConfigurationManager.AppSettings["SITENAME"].ToString();
            string SITEKEY  = ConfigurationManager.AppSettings["SITEKEY"].ToString();
            var Value = HttpContext.Current.Request.Cookies[Key + "_" + SITENAME]?.Value.Decrypt();
            return Value;
        }
        public static void PostCookies(string Key, string Value)
        {
            string SITENAME = ConfigurationManager.AppSettings["SITENAME"].ToString();
            string SITEKEY = ConfigurationManager.AppSettings["SITEKEY"].ToString();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(Key + "_" + SITENAME, Value.Encrypt()));
        }
        public static string GetCookiesWithoutEnc(string Key)
        {
            string SITENAME = ConfigurationManager.AppSettings["SITENAME"].ToString();
            string SITEKEY = ConfigurationManager.AppSettings["SITEKEY"].ToString();
            var Value = HttpContext.Current.Request.Cookies[Key]?.Value;
            return Value;
        }
        public static void PostCookiesWithoutEnc(string Key, string Value)
        {
            string SITENAME = ConfigurationManager.AppSettings["SITENAME"].ToString();
            string SITEKEY = ConfigurationManager.AppSettings["SITEKEY"].ToString();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(Key + "_" + SITENAME, Value));
        }
        public static void SetExpire(string key, double day)
        {
            string SITENAME = ConfigurationManager.AppSettings["SITENAME"].ToString();
            string SITEKEY = ConfigurationManager.AppSettings["SITEKEY"].ToString();
            HttpContext.Current.Response.Cookies[key + "_" + SITENAME].Expires = DateTime.Now.AddDays(day);
        }
        public static void DeleteCookies(string Key)
        {
            string SITENAME = ConfigurationManager.AppSettings["SITENAME"].ToString();
            string SITEKEY = ConfigurationManager.AppSettings["SITEKEY"].ToString();
            HttpContext.Current.Response.Cookies[Key + "_" + SITENAME].Expires = DateTime.Now.AddDays(-1);
        }
    }
}