using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SBM_POWER_BI.Classes;
using SBM_POWER_BI.Models;
using System;
using System.DirectoryServices;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using SBM_Captcha_ASP;
using System.IO;
using TwoFactorAuthNet;
using TwoFactorAuthNet.QRCoder;

namespace SBM_POWER_BI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        readonly MASTERDATA MD = new MASTERDATA();
        readonly TwoFactorAuthenticate TFAM = new TwoFactorAuthenticate();
        readonly JwtTokenGenerator JWT = new JwtTokenGenerator();

        private readonly SBMCaptcha _captcha;
        HttpRequest request = System.Web.HttpContext.Current.Request;
        HttpResponse response = System.Web.HttpContext.Current.Response;

        public AccountController()
        {
            ISessionProvider sessionProvider = new HttpContextSession();
            string _contentFolder = @"Content\icons";
            string pathToContent = AppDomain.CurrentDomain.BaseDirectory + _contentFolder;
            _captcha = new SBMCaptcha(sessionProvider, pathToContent);
        }

        [AllowAnonymous]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public object GetCaptcha()
        {
            SBMCaptchaResult result = SBMCaptchaExtension.CallSBMCaptcha(_captcha, request, response);
            switch (result.CaptchaState)
            {
                case SBMCaptchaState.CaptchaHashesReturned:
                    return Json(result.CaptchaResult as string[], JsonRequestBehavior.AllowGet);
                case SBMCaptchaState.CaptchaImageReturned:
                    return File(result.CaptchaResult as FileStream, "image/png");
                case SBMCaptchaState.CaptchaIconSelected:
                case SBMCaptchaState.CaptchaGeneralFail:
                default:
                    return null;
            }
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (JWT.IsAuth())
            {
                return RedirectToAction("Dashboard", "Report");
            }
            else
            {
                HttpCookieCollection cookies = Request.Cookies;
                foreach (string cookieName in cookies.AllKeys)
                {
                    if (!cookieName.StartsWith("__RequestVerificationToken") && !cookieName.Contains("ASP.NET_SessionId"))
                    {
                        HttpCookie cookie = new HttpCookie(cookieName)
                        {
                            Expires = DateTime.Now.AddDays(-1)
                        };
                        Response.Cookies.Add(cookie);
                    }
                }
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            HttpCookieCollection cookies = Request.Cookies;
            foreach (string cookieName in cookies.AllKeys)
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[CaptchaValidationActionFilter("CaptchaCode", "ExampleCaptcha", "Incorrect!")]

        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //MvcCaptcha.ResetCaptcha("ExampleCaptcha");
            try
            {
                HttpRequest request = System.Web.HttpContext.Current.Request;
                var OK = _captcha.ValidateSubmission(request);
                if (!ModelState.IsValid)
                {
                    var ERROR = "INVALID CREDENTIALS";
                    TempData["FAILED"] = ERROR;
                    ModelState.AddModelError("", ERROR);
                    return View(model);
                }
                var Result = MD.GET_USER_INFO(model.WINDOWS_ID);
                if (Result.Count == 0)
                {
                    var ERROR = "DATA NOT FOUND ON MDM";
                    TempData["FAILED"] = ERROR;
                    ModelState.AddModelError("", ERROR);
                    return View(model);
                }
                UserInfo userInfo = GetUserInfo(model.WINDOWS_ID, model.PASSWORD, "SHIMANOACE");
                if (string.IsNullOrEmpty(userInfo.Email.ToLower()))
                {
                    var ERROR = "DATA NOT FOUND ON ACTIVE DIRECTORY";
                    TempData["FAILED"] = ERROR;
                    ModelState.AddModelError("", ERROR);
                    return View(model);
                }
                CookieContainer cookieContainer = new CookieContainer();
                //bool isCookieAvailable = IsCookieAvailable(cookieContainer, "app.powerbi.com", "SignInStateCookie");

                Session["FULL_NAME"] = Result[0].FULL_NAME.ToUpper();
                Session["USERID"] = Result[0].USERID.ToUpper();
                Session["WINDOWSID"] = Result[0].WINDOWSID.ToUpper();
                Session["NAME"] = Result[0].FULL_NAME.ToUpper();
                Session["EMAIL"] = Result[0].EMAIL.ToUpper();
                Session["DEPARTMENT"] = Result[0].DEPT.ToUpper();
                Session["GROUPS_MDM"] = Result[0].GROUPS_MDM.ToUpper();
                Session["GROUPS_PBI"] = Result[0].GROUPS_PBI.ToUpper();

                string INITIALS = GetFirstAndLastInitials(Result[0].FULL_NAME.ToUpper());
                COOKIES.PostCookies("USERID", Result[0].USERID.ToLower());
                COOKIES.PostCookies("WINDOWSID", Result[0].WINDOWSID.ToLower());
                COOKIES.PostCookies("NAME", Result[0].FULL_NAME.ToUpper());
                COOKIES.PostCookies("EMAIL", Result[0].EMAIL.ToUpper());
                COOKIES.PostCookies("DEPARTMENT", Result[0].DEPT.ToUpper());
                COOKIES.PostCookies("GROUPS_MDM", Result[0].GROUPS_MDM.ToUpper());
                COOKIES.PostCookies("GROUPS_PBI", Result[0].GROUPS_PBI.ToUpper());
                COOKIES.PostCookies("INITIALS", INITIALS);
                COOKIES.SetExpire("USERID", 1);
                COOKIES.SetExpire("WINDOWSID", 1);
                COOKIES.SetExpire("NAME", 1);
                COOKIES.SetExpire("EMAIL", 1);
                COOKIES.SetExpire("DEPARTMENT", 1);
                COOKIES.SetExpire("GROUPS_MDM", 1);
                COOKIES.SetExpire("GROUPS_PBI", 1);
                COOKIES.SetExpire("INITIALS", 1);
                JwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator();
                var USERID = COOKIES.GetCookies("USERID").ToString();
                var WINDOWSID = COOKIES.GetCookies("WINDOWSID").ToString();
                var NAME = COOKIES.GetCookies("NAME").ToString();
                var EMAIL = COOKIES.GetCookies("EMAIL").ToString();
                var DEPARTMENT = COOKIES.GetCookies("DEPARTMENT").ToString();
                var GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM").ToString();
                var GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI").ToString();
                var KEY = GenerateStrongKey(USERID, WINDOWSID, NAME, EMAIL, DEPARTMENT, GROUPS_MDM, GROUPS_PBI);
                GROUPS_PBI = (KEY + GROUPS_PBI + KEY);
                DEPARTMENT = (KEY + DEPARTMENT + KEY);
                var TOKEN = jwtTokenGenerator.GenerateToken(USERID, DEPARTMENT, GROUPS_PBI);
                COOKIES.PostCookies("TOKEN", TOKEN);
                //////
                COOKIES.PostCookies("returnUrl", (returnUrl ?? ""));
                COOKIES.SetExpire("returnUrl", 1);
                var CS_TFA = CheckSystemTFA();
                return RedirectToLocal(returnUrl);
                //////
            }
            catch (SBMCaptchaException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        //public ActionResult Login(LoginModel model, string returnUrl)
        //{
        //    //MvcCaptcha.ResetCaptcha("ExampleCaptcha");
        //    try
        //    {
        //        HttpRequest request = System.Web.HttpContext.Current.Request;
        //        var OK = _captcha.ValidateSubmission(request);
        //        if (!ModelState.IsValid)
        //        {
        //            var ERROR = "INVALID CREDENTIALS";
        //            TempData["FAILED"] = ERROR;
        //            ModelState.AddModelError("", ERROR);
        //            return View(model);
        //        }
        //        var Result = MD.GET_USER_INFO(model.WINDOWS_ID);
        //        if (Result.Count == 0)
        //        {
        //            var ERROR = "DATA NOT FOUND ON MDM";
        //            TempData["FAILED"] = ERROR;
        //            ModelState.AddModelError("", ERROR);
        //            return View(model);
        //        }
        //        UserInfo userInfo = GetUserInfo(model.WINDOWS_ID, model.PASSWORD, "SHIMANOACE");
        //        if (string.IsNullOrEmpty(userInfo.Email.ToLower()))
        //        {
        //            var ERROR = "DATA NOT FOUND ON ACTIVE DIRECTORY";
        //            TempData["FAILED"] = ERROR;
        //            ModelState.AddModelError("", ERROR);
        //            return View(model);
        //        }
        //        CookieContainer cookieContainer = new CookieContainer();
        //        //bool isCookieAvailable = IsCookieAvailable(cookieContainer, "app.powerbi.com", "SignInStateCookie");

        //        Session["FULL_NAME"] = Result[0].FULL_NAME.ToUpper();
        //        Session["USERID"] = Result[0].USERID.ToUpper();
        //        Session["WINDOWSID"] = Result[0].WINDOWSID.ToUpper();
        //        Session["NAME"] = Result[0].FULL_NAME.ToUpper();
        //        Session["EMAIL"] = Result[0].EMAIL.ToUpper();
        //        Session["DEPARTMENT"] = Result[0].DEPT.ToUpper();
        //        Session["GROUPS_MDM"] = Result[0].GROUPS_MDM.ToUpper();
        //        Session["GROUPS_PBI"] = Result[0].GROUPS_PBI.ToUpper();

        //        //string INITIALS = GetFirstAndLastInitials(Result[0].FULL_NAME.ToUpper());
        //        COOKIES.PostCookies("USERID", Result[0].USERID.ToLower());
        //        //COOKIES.PostCookies("WINDOWSID", Result[0].WINDOWSID.ToLower());
        //        //COOKIES.PostCookies("NAME", Result[0].FULL_NAME.ToUpper());
        //        COOKIES.PostCookies("EMAIL", Result[0].EMAIL.ToUpper());
        //        //COOKIES.PostCookies("DEPARTMENT", Result[0].DEPT.ToUpper());
        //        //COOKIES.PostCookies("GROUPS_MDM", Result[0].GROUPS_MDM.ToUpper());
        //        //COOKIES.PostCookies("GROUPS_PBI", Result[0].GROUPS_PBI.ToUpper());
        //        //COOKIES.PostCookies("INITIALS", INITIALS);
        //        COOKIES.SetExpire("USERID", 1);
        //        //COOKIES.SetExpire("WINDOWSID", 1);
        //        //COOKIES.SetExpire("NAME", 1);
        //        COOKIES.SetExpire("EMAIL", 1);
        //        //COOKIES.SetExpire("DEPARTMENT", 1);
        //        //COOKIES.SetExpire("GROUPS_MDM", 1);
        //        //COOKIES.SetExpire("GROUPS_PBI", 1);
        //        //COOKIES.SetExpire("INITIALS", 1);
        //        //JwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator();
        //        //var USERID = COOKIES.GetCookies("USERID").ToString();
        //        //var WINDOWSID = COOKIES.GetCookies("WINDOWSID").ToString();
        //        //var NAME = COOKIES.GetCookies("NAME").ToString();
        //        //var EMAIL = COOKIES.GetCookies("EMAIL").ToString();
        //        //var DEPARTMENT = COOKIES.GetCookies("DEPARTMENT").ToString();
        //        //var GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM").ToString();
        //        //var GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI").ToString();
        //        //var KEY = GenerateStrongKey(USERID, WINDOWSID, NAME, EMAIL, DEPARTMENT, GROUPS_MDM, GROUPS_PBI);
        //        //GROUPS_PBI = (KEY + GROUPS_PBI + KEY);
        //        //DEPARTMENT = (KEY + DEPARTMENT + KEY);
        //        //var TOKEN = jwtTokenGenerator.GenerateToken(USERID, DEPARTMENT, GROUPS_PBI);
        //        //COOKIES.PostCookies("TOKEN", TOKEN);
        //        //////
        //        COOKIES.PostCookies("returnUrl", (returnUrl ?? ""));
        //        COOKIES.SetExpire("returnUrl", 1);
        //        var CS_TFA = CheckSystemTFA();
        //        if (CS_TFA)
        //        {
        //            return RedirectToAction("CreateTFA", "Account");
        //        }
        //        else
        //        {
        //            return RedirectToLocal(returnUrl);
        //        }
        //        //////
        //    }
        //    catch (SBMCaptchaException ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        return View(model);
        //    }
        //}

        [AllowAnonymous]
        public ActionResult CreateTFA()
        {
            var TFAS = new TwoFactorAuth("PT. SHIMANO BATAM", qrcodeprovider: new QRCoderQRCodeProvider());
            var SECRET_CODE = TFAS.CreateSecret();
            var SECRET_QR = TFAS.GetQrCodeImageAsDataUri("SBM POWER BI", SECRET_CODE);
            var SECRET_URL = TFAS.GetQrText("SBM POWER BI", SECRET_CODE);
            var CU_TFA = CheckUserTFA();
            if (!CU_TFA)
            {
                COOKIES.PostCookies("SECRET_QR", SECRET_QR);
                COOKIES.PostCookies("SECRET_CODE", SECRET_CODE);
                COOKIES.PostCookies("SECRET_URL", SECRET_URL);
                COOKIES.SetExpire("SECRET_QR", 1);
                COOKIES.SetExpire("SECRET_CODE", 1);
                COOKIES.SetExpire("SECRET_URL", 1);
                ViewBag.SECRET_QR = COOKIES.GetCookies("SECRET_QR").ToString();
                ViewBag.SECRET_CODE = COOKIES.GetCookies("SECRET_CODE").ToString();
                ViewBag.SECRET_URL = COOKIES.GetCookies("SECRET_URL").ToString();
                COOKIES.PostCookies("TFA_EXITS_ON_SQL", "false");
                COOKIES.SetExpire("TFA_EXITS_ON_SQL", 1);
                return View();
            }
            else
            {
                COOKIES.PostCookies("TFA_EXITS_ON_SQL", "true");
                COOKIES.SetExpire("TFA_EXITS_ON_SQL", 1);
                return RedirectToAction("Verify", "Account");
            }
        }

        [AllowAnonymous]
        public ActionResult Verify()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Verify(MODEL_TFA model)
        {
            var TFA_EXITS = (COOKIES.GetCookies("TFA_EXITS_ON_SQL") ?? "");
            var result = false;
            var SECRET_QR = COOKIES.GetCookies("SECRET_QR").ToString();
            var SECRET_CODE = COOKIES.GetCookies("SECRET_CODE").ToString();
            var SECRET_URL = COOKIES.GetCookies("SECRET_URL").ToString();

            if (TFA_EXITS == "false")
            {
                ADD_USER_TFA MODEL = new ADD_USER_TFA
                {
                    USERID = COOKIES.GetCookies("USERID").ToString(),
                    EMAIL = COOKIES.GetCookies("EMAIL").ToString(),
                    APPLICATION = "SBM POWER BI",
                    SECRET_CODE = SECRET_CODE,
                    SECRET_URL = SECRET_URL,
                    SECRET_TEXT = SECRET_QR
                };
                var res = TFAM.ADD_USER_TFA(MODEL);
                if (res.Result)
                {
                    if (res.Data.Count > 0)
                    {
                        var tfa = new TwoFactorAuth("PT. SHIMANO BATAM", qrcodeprovider: new QRCoderQRCodeProvider());
                        result = tfa.VerifyCode(res.Data[0].SECRET_CODE, model.CODE);
                    }
                }
            }
            else
            {
                var tfa = new TwoFactorAuth("PT. SHIMANO BATAM", qrcodeprovider: new QRCoderQRCodeProvider());
                result = tfa.VerifyCode(SECRET_CODE, model.CODE);
            }

            if (result)
            {
                string INITIALS = GetFirstAndLastInitials(Session["FULL_NAME"].ToString());

                string WINDOWSIDs = Session["WINDOWSID"].ToString();
                string NAMEs = Session["NAME"].ToString();
                string DEPARTMENTs = Session["DEPARTMENT"].ToString();
                string GROUPS_MDMs = Session["GROUPS_MDM"].ToString();
                string GROUPS_PBIs = Session["GROUPS_PBI"].ToString();

                //COOKIES.PostCookies("USERID", Session["USERID"].ToString());
                COOKIES.PostCookies("WINDOWSID", WINDOWSIDs);
                COOKIES.PostCookies("NAME", NAMEs);
                //COOKIES.PostCookies("EMAIL", Session["EMAIL"].ToString());
                COOKIES.PostCookies("DEPARTMENT", DEPARTMENTs);
                COOKIES.PostCookies("GROUPS_MDM", GROUPS_MDMs);
                COOKIES.PostCookies("GROUPS_PBI", GROUPS_PBIs);
                COOKIES.PostCookies("INITIALS", INITIALS);
                //COOKIES.SetExpire("USERID", 1);
                COOKIES.SetExpire("WINDOWSID", 1);
                COOKIES.SetExpire("NAME", 1);
                //COOKIES.SetExpire("EMAIL", 1);
                COOKIES.SetExpire("DEPARTMENT", 1);
                COOKIES.SetExpire("GROUPS_MDM", 1);
                COOKIES.SetExpire("GROUPS_PBI", 1);
                COOKIES.SetExpire("INITIALS", 1);
                JwtTokenGenerator jwtTokenGenerator = new JwtTokenGenerator();
                var USERID = COOKIES.GetCookies("USERID").ToString();
                var WINDOWSID = COOKIES.GetCookies("WINDOWSID").ToString();
                var NAME = COOKIES.GetCookies("NAME").ToString();
                var EMAIL = COOKIES.GetCookies("EMAIL").ToString();
                var DEPARTMENT = COOKIES.GetCookies("DEPARTMENT").ToString();
                var GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM").ToString();
                var GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI").ToString();
                var KEY = GenerateStrongKey(USERID, WINDOWSID, NAME, EMAIL, DEPARTMENT, GROUPS_MDM, GROUPS_PBI);
                GROUPS_PBI = (KEY + GROUPS_PBI + KEY);
                DEPARTMENT = (KEY + DEPARTMENT + KEY);
                var TOKEN = jwtTokenGenerator.GenerateToken(USERID, DEPARTMENT, GROUPS_PBI);
                COOKIES.PostCookies("TOKEN", TOKEN);
                var returnUrl = COOKIES.GetCookies("returnUrl");
                return RedirectToLocal(returnUrl);
                //return View();
            }
            else
            {
                return View();
            }
        }

        [AllowAnonymous]
        public bool CheckSystemTFA()
        {
            try
            {
                ADD_SYSTEM_TFA MODEL = new ADD_SYSTEM_TFA
                {
                    APPLICATION = "SBM POWER BI",
                };
                var res = TFAM.ADD_SYSTEM_TFA(MODEL);
                if (res.Result)
                {
                    return res.Data[0].IS_REQUIRE;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [AllowAnonymous]
        public bool CheckUserTFA()
        {
            ADD_USER_TFA MODEL = new ADD_USER_TFA
            {
                USERID = COOKIES.GetCookies("USERID").ToString(),
                EMAIL = COOKIES.GetCookies("EMAIL").ToString(),
                APPLICATION = "SBM POWER BI",
            };
            var res = TFAM.GET_USER_TFA(MODEL);
            if (res.Result)
            {
                if (res.Data.Count > 0)
                {
                    COOKIES.PostCookies("SECRET_QR", res.Data[0].SECRET_TEXT);
                    COOKIES.PostCookies("SECRET_CODE", res.Data[0].SECRET_CODE);
                    COOKIES.PostCookies("SECRET_URL", res.Data[0].SECRET_URL);
                    COOKIES.SetExpire("SECRET_QR", 1);
                    COOKIES.SetExpire("SECRET_CODE", 1);
                    COOKIES.SetExpire("SECRET_URL", 1);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        




        static string GenerateStrongKey(params string[] values)
        {
            var concatenatedValues = string.Join("|", values);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(concatenatedValues));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public UserInfo GetUserInfo(string userName, string password, string domain)
        {
            UserInfo userInfo = new UserInfo();
            using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain, userName, password))
            {
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
                searcher.PropertiesToLoad.Add("displayName");
                searcher.PropertiesToLoad.Add("mail");
                searcher.PropertiesToLoad.Add("department");
                searcher.PropertiesToLoad.Add("title");
                searcher.PropertiesToLoad.Add("telephoneNumber");
                searcher.PropertiesToLoad.Add("company");
                searcher.PropertiesToLoad.Add("co");
                //searcher.PropertiesToLoad.Add("manager"); // User's manager
                //searcher.PropertiesToLoad.Add("employeeID"); // User's employee ID
                searcher.PropertiesToLoad.Add("physicalDeliveryOfficeName");
                SearchResult result = searcher.FindOne();
                if (result != null)
                {
                    userInfo.Name = result.Properties["displayName"][0].ToString() ?? "";
                    userInfo.Email = result.Properties["mail"][0].ToString() ?? "";
                    userInfo.Department = result.Properties["department"][0].ToString() ?? "";
                    userInfo.Title = "";
                    userInfo.PhoneNumber = "";
                    userInfo.Company = result.Properties["company"][0].ToString() ?? "";
                    userInfo.Country = result.Properties["co"][0].ToString() ?? "";
                    //userInfo.Manager = result.Properties["manager"][0].ToString() ?? "";
                    //userInfo.EmployeeID = result.Properties["employeeID"][0].ToString() ?? "";
                    userInfo.Office = "";
                }
            }
            return userInfo;
        }

        static bool IsCookieAvailable(CookieContainer cookieContainer, string domain, string cookieName)
        {
            CookieCollection cookies = cookieContainer.GetCookies(new Uri($"https://{domain}"));

            foreach (Cookie cookie in cookies)
            {
                if (cookie.Name == cookieName)
                {
                    return true;
                }
            }
            return false;
        }

        static string GetFirstAndLastInitials(string fullName)
        {
            string[] words = fullName.Split(' ');
            StringBuilder initialsBuilder = new System.Text.StringBuilder();
            if (words.Length > 0 && !string.IsNullOrEmpty(words[0]))
            {
                initialsBuilder.Append(words[0][0]);
            }
            if (words.Length > 1 && !string.IsNullOrEmpty(words[words.Length - 1]))
            {
                initialsBuilder.Append(words[words.Length - 1][0]);
            }
            string initials = initialsBuilder.ToString().ToUpper();
            return initials;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Dashboard", "Report");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
    public class HttpContextSession : ISessionProvider
    {
        public SBMCaptchaSession GetSession(string key)
        {
            return (SBMCaptchaSession)HttpContext.Current.Session[key];
        }
        public void SetSession(string key, SBMCaptchaSession value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}