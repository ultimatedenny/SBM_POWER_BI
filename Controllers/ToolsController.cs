using System.Web.Mvc;

namespace SBM_POWER_BI.Controllers
{
    [JwtAuthorize]
    public class ToolsController : Controller
    {
        public ActionResult DownloadFile()
        {
            return View();
        }
    }
}