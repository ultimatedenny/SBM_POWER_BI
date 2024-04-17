using SBM_POWER_BI;
using System.Web;
using System;
using System.Web.Mvc;
using SBM_POWER_BI.Classes;
using System.Reflection;
using SBM_POWER_BI.Models;

namespace SBM_POWER_BI.Controllers
{
    [JwtAuthorize]
    public class ProfileController : Controller
    {
        readonly MASTERDATA MD = new MASTERDATA();
        public ActionResult Index()
        {
            GetCookies();
            return View();
        }
        public ActionResult Link()
        {
            GetCookies();
            return View();
        }
        public JsonResult GET_MY_LINK()
        {
            try
            {
                return Json(new { data = MD.GET_MY_LINK() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_MY_LOG_LINK()
        {
            try
            {
                return Json(new { data = MD.GET_MY_LOG_LINK() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EDIT_TIME(PBI_SHARE_URL MODEL)
        {
            try
            {
                return Json(new { data = MD.EDIT_TIME(MODEL) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public void GetCookies()
        {
            ViewBag.USERID = COOKIES.GetCookies("USERID") ?? "";
            ViewBag.WINDOWSID = COOKIES.GetCookies("WINDOWSID") ?? "";
            ViewBag.NAME = COOKIES.GetCookies("NAME") ?? "";
            ViewBag.EMAIL = COOKIES.GetCookies("EMAIL") ?? "";
            ViewBag.DEPARTMENT = COOKIES.GetCookies("DEPARTMENT") ?? "";
            ViewBag.GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM") ?? "";
            ViewBag.GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI") ?? "";
            ViewBag.INITIALS = COOKIES.GetCookies("INITIALS") ?? "";

            Assembly assembly = Assembly.GetExecutingAssembly();
            string versions = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? "Unknown";
            string appNames = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "Unknown";
            string companys = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "Unknown";
            var BT = System.IO.File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);
            string BUILDTIME = BT.ToString("dd MMM yyyy HH:mm:ss tt").ToUpper();
            string headers = $"{appNames} © {DateTime.Now.Year} {companys}";

            ViewBag.BUILDTIME = BUILDTIME;
            ViewBag.INFO = headers;
            ViewBag.APPNAME = appNames;
            ViewBag.VERSION = versions;
            ViewBag.COMPANY = companys;
        }
    }
}