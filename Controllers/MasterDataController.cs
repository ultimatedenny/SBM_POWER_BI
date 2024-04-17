using SBM_POWER_BI.Classes;
using SBM_POWER_BI.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Web.ModelBinding;

namespace SBM_POWER_BI.Controllers
{
    [JwtAuthorize]
    public class MasterDataController : Controller
    {
        readonly MASTERDATA MD = new MASTERDATA();

        [GroupAuthorize(GroupCookieName = "GROUPS_PBI")]
        public ActionResult Report()
        {
            GetCookies();
            return View();
        }

        [GroupAuthorize(GroupCookieName = "GROUPS_PBI")]
        public ActionResult Category()
        {
            GetCookies();
            return View();
        }

        [GroupAuthorize(GroupCookieName = "GROUPS_PBI")]
        public ActionResult Department()
        {
            GetCookies();
            return View();
        }

        [GroupAuthorize(GroupCookieName = "GROUPS_PBI")]
        public ActionResult Group()
        {
            GetCookies();
            return View();
        }

        [GroupAuthorize(GroupCookieName = "GROUPS_PBI")]
        public ActionResult UserDC()
        {
            GetCookies();
            return View();
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


        public JsonResult GET_REPORT()
        {
            try
            {
                return Json(new { data = MD.GET_REPORT() }, JsonRequestBehavior.AllowGet);
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
        public JsonResult ADD_REPORT(PBI_REPORT MODEL)
        {
            try
            {
                var res = MD.ADD_REPORT(MODEL);
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
        public JsonResult DETAIL_REPORT(PBI_REPORT MODEL)
        {
            try
            {
                var res = MD.DETAIL_REPORT(MODEL);
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
        public JsonResult EDIT_REPORT(PBI_REPORT MODEL)
        {
            try
            {
                var res = MD.EDIT_REPORT(MODEL);
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
        public JsonResult DELETE_REPORT(PBI_REPORT MODEL)
        {
            try
            {
                var res = MD.DELETE_REPORT(MODEL);
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

        
        public JsonResult GET_CATEGORY()
        {
            try
            {
                return Json(new { data = MD.GET_CATEGORY() }, JsonRequestBehavior.AllowGet);
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
        public JsonResult ADD_CATEGORY(PBI_CATEGORY MODEL)
        {
            try
            {
                var res = MD.ADD_CATEGORY(MODEL);
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
        public JsonResult DETAIL_CATEGORY(PBI_CATEGORY MODEL)
        {
            try
            {
                var res = MD.DETAIL_CATEGORY(MODEL);
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
        public JsonResult EDIT_CATEGORY(PBI_CATEGORY MODEL)
        {
            try
            {
                var res = MD.EDIT_CATEGORY(MODEL);
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
        public JsonResult DELETE_CATEGORY(PBI_CATEGORY MODEL)
        {
            try
            {
                var res = MD.DELETE_CATEGORY(MODEL);
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

        
        public JsonResult GET_DEPARTMENT()
        {
            try
            {
                return Json(new { data = MD.GET_DEPARTMENT() }, JsonRequestBehavior.AllowGet);
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
        public JsonResult ADD_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            try
            {
                var res = MD.ADD_DEPARTMENT(MODEL);
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
        public JsonResult DETAIL_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            try
            {
                var res = MD.DETAIL_DEPARTMENT(MODEL);
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
        public JsonResult EDIT_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            try
            {
                var res = MD.EDIT_DEPARTMENT(MODEL);
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
        public JsonResult DELETE_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            try
            {
                var res = MD.DELETE_DEPARTMENT(MODEL);
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


        public JsonResult GET_GROUP()
        {
            try
            {
                return Json(new { data = MD.GET_GROUP() }, JsonRequestBehavior.AllowGet);
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
        public JsonResult ADD_GROUP(PBI_GROUP MODEL)
        {
            try
            {
                var res = MD.ADD_GROUP(MODEL);
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
        public JsonResult DETAIL_GROUP(PBI_GROUP MODEL)
        {
            try
            {
                var res = MD.DETAIL_GROUP(MODEL);
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
        public JsonResult EDIT_GROUP(PBI_GROUP MODEL)
        {
            try
            {
                var res = MD.EDIT_GROUP(MODEL);
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
        public JsonResult DELETE_GROUP(PBI_GROUP MODEL)
        {
            try
            {
                var res = MD.DELETE_GROUP(MODEL);
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


        public JsonResult GET_USER_DC()
        {
            try
            {
                return Json(new { data = MD.GET_USER_DC() }, JsonRequestBehavior.AllowGet);
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
        public JsonResult DETAIL_USER_DC(PBI_USER_DC MODEL)
        {
            try
            {
                var res = MD.DETAIL_USER_DC(MODEL);
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
        public JsonResult UPDATE_USER_GROUP(PBI_USER MODEL)
        {
            try
            {
                var res = MD.UPDATE_USER_GROUP(MODEL);
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



        public JsonResult SYNC_DEPARTMENT()
        {
            try
            {
                var res = MD.SYNC_DEPARTMENT();
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
        public JsonResult SYNC_USER_DC()
        {
            try
            {
                var res = MD.RETEIVE_USER_DC();
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
        public JsonResult GET_RECENT()
        {
            try
            {
                var DEPARTMENT = COOKIES.GetCookies("DEPARTMENT").ToString();
                ViewBag.DEPARTMENT = "."+DEPARTMENT;
                return Json(new { data = MD.GET_RECENT() }, JsonRequestBehavior.AllowGet);
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
        public JsonResult UPDATE_TOTAL_VIEW(PBI_REPORT MODEL)
        {
            try
            {
                var res = MD.UPDATE_TOTAL_VIEW(MODEL, "");
                COOKIES.PostCookies("REPORT_URL", res.Message.ToString());
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
        public JsonResult UPDATE_TOTAL_VIEW_2(PBI_REPORT MODEL)
        {
            try
            {
                var res = MD.UPDATE_TOTAL_VIEW(MODEL, "");
                COOKIES.PostCookies("REPORT_URL", res.Message.ToString());
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
        public JsonResult GET_MENU_LIST()
        {
            try
            {
                var res = MD.GET_MENU_LIST();
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
        public JsonResult GET_URL(PBI_SHARE_URL MODEL)
        {
            try
            {
                var URL_PASS  = GenerateAesKey(256);
                //var URL_TXT   = Encrypt(MODEL.REPORT_ID, URL_PASS);
                var URL_TXT   = HttpUtility.UrlEncode(Encrypt(MODEL.REPORT_ID, URL_PASS));
                var URL_TYPE  = MODEL.URL_TYPE;
                var URL_EXPI  = MODEL.URL_EXPI ?? "3999-12-31";
                var REPORT_ID = MODEL.REPORT_ID;
                var res       = MD.INSERT_PBI_SHARE_URL(REPORT_ID, URL_PASS, URL_TYPE, URL_TXT, URL_EXPI, "");
               
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
        public JsonResult GET_PERMISSION()
        {
            try
            {
                var res = MD.GET_PERMISSION();
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


        public static string GenerateStrongKey(params string[] values)
        {
            var concatenatedValues = string.Join("|", values);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenatedValues));
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static string Random128()
        {
            const int byteLength = 256;
            byte[] randomBytes = new byte[byteLength];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
        }
        public static string GenerateAesKey(int keySizeInBits)
        {
            if (keySizeInBits != 128 && keySizeInBits != 192 && keySizeInBits != 256)
            {
                throw new ArgumentException("Invalid key size. Valid sizes are 128, 192, or 256 bits.");
            }
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] keyBytes = new byte[keySizeInBits / 8];
                rng.GetBytes(keyBytes);
                return Convert.ToBase64String(keyBytes);
            }
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