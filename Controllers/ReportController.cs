using System.IO;
using System.Security.Cryptography;
using System;
using System.Web.Mvc;
using SBM_POWER_BI.Classes;
using System.Linq;
using SBM_POWER_BI.Models;
using System.Web;
using System.Reflection;

namespace SBM_POWER_BI.Controllers
{
    [JwtAuthorize]
    public class ReportController : Controller
    {
        readonly JwtTokenGenerator JWT = new JwtTokenGenerator();
        readonly MasterDataController MD = new MasterDataController();

        public ActionResult Dashboard()
        {
            try
            {
                GetCookies();
                return View();
            }
            catch (Exception)
            {
                return View("Index", "Error");
            }
        }

        public ActionResult DashboardList()
        {
            try
            {
                GetCookies();
                return View();
            }
            catch (Exception)
            {
                return View("Index", "Error");
            }
        }

        public ActionResult Item()
        {
            try
            {
                GetCookies();
                return View();
            }
            catch (Exception)
            {
                return View("Index", "Error");
            }
        }

        public ActionResult Detail()
        {
            try
            {
                GetCookies();
                ViewBag.REPORT_URL = COOKIES.GetCookies("REPORT_URL");
                return View();
            }
            catch (Exception)
            {
                return View("Index", "Error");
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

        public ActionResult Shared(string ID)
        {
            MASTERDATA MD = new MASTERDATA();
            var URL_TYPE = "0";
            var USER_ID = GetUserIpAddress();
            System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(USER_ID);
            string MACHINE = entry.HostName.ToUpper() + " (" + (COOKIES.GetCookies("NAME") ?? "COOKIES EXPIRE") + ")";
            //System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(USER_ID);
            //clientMachineName = entry.HostName;
            var res1 = MD.GET_PBI_SHARE_URL(URL_TYPE, HttpUtility.UrlEncode(ID), "");
            if(res1[0].REPORT_ID != "")
            {
                var REPORT_ID = Decrypt(ID, res1[0].REPORT_ID);
                PBI_REPORT MODEL = new PBI_REPORT
                {
                    ID = REPORT_ID,
                };

                var res2 = MD.UPDATE_TOTAL_VIEW(MODEL, res1[0].URL_TYPE);
                MD.LOG_URL(res1[0].REPORT_ID, USER_ID, MACHINE);
                ViewBag.REPORT_URL = res2.Message.ToString();
                return View();
            }
            else
            {
                return RedirectToAction("Index","Error");
            }
        }


        [AllowAnonymous]
        public ActionResult SharedTV(string ID)
        {
            MASTERDATA MD = new MASTERDATA();
            var URL_TYPE = "1";
            var USER_ID = GetUserIpAddress();
            System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(USER_ID);
            string MACHINE = entry.HostName.ToUpper() + " (" + (COOKIES.GetCookies("NAME") ?? "COOKIES EXPIRE") + ")";
            var res1 = MD.GET_PBI_SHARE_URL(URL_TYPE, HttpUtility.UrlEncode(ID), "");
            var REPORT_ID = Decrypt(ID, res1[0].REPORT_ID);
            PBI_REPORT MODEL = new PBI_REPORT
            {
                ID = REPORT_ID,
            };
            COOKIES.PostCookies("URL_TYPE", res1[0].URL_TYPE);
            var res2 = MD.UPDATE_TOTAL_VIEW(MODEL, res1[0].URL_TYPE);
            MD.LOG_URL(res1[0].REPORT_ID, USER_ID, MACHINE);
            ViewBag.REPORT_ID = REPORT_ID;
            ViewBag.REPORT_URL = res2.Message.ToString();
            return View();
        }

        [AllowAnonymous]
        public JsonResult LOAD_FRAME(PBI_REPORT MODEL)
        {
            try
            {
                MASTERDATA MD = new MASTERDATA();
                var URL_TYPE  = COOKIES.GetCookies("URL_TYPE");
                var res = MD.UPDATE_TOTAL_VIEW_NEW(MODEL, URL_TYPE);
                return Json(new { res }, JsonRequestBehavior.AllowGet);
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


        private string GetUserIpAddress()
        {
            string ipAddress = HttpContext.Request.UserHostAddress;
            if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
            {
                string forwardedFor = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    string[] forwardedIps = forwardedFor.Split(',');
                    if (forwardedIps.Length > 0)
                    {
                        ipAddress = forwardedIps[0].Trim();
                    }
                }
            }
            if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
            {
                ipAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ipAddress;
        }

        public static string Encrypt(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.GenerateIV();
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    byte[] encryptedDataWithIV = aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray();
                    return Convert.ToBase64String(encryptedDataWithIV);
                }
            }
        }

        public static string Decrypt(string cipherText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(key);
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] encryptedData = Convert.FromBase64String(cipherText);
                Array.Copy(encryptedData, iv, iv.Length);
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}