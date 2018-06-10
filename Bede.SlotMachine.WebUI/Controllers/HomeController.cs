using System.Linq;
using System.Web.Mvc;

namespace Bede.SlotMachine.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private static Engine.SlotEngine slotEngine;

        static HomeController()
        {
            slotEngine = new Engine.SlotEngine();
        }

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            var lastHistoryItem = slotEngine.SpinHistory.LastOrDefault();
            ViewBag.LastSpin = lastHistoryItem;

            return View();
        }

        [HttpPost]
        public JsonResult EnterDeposit(double? deposit)
        {
            if(!deposit.HasValue)
            {
                return Json(new { OK = false, Message = "Value is NULL!" }, JsonRequestBehavior.AllowGet);
            }

            slotEngine.EnterDeposit(deposit.Value);

            return Json(new { OK = true, Message = "Deposit entered" }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetCurrentState()
        //{

        //    var lastHistoryItem = slotEngine.SpinHistory.Last();

        //    if (lastHistoryItem == null)
        //    {
        //        return Json(new { OK = false, Message = "Not enough balance!", Data = lastHistoryItem }, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(new { OK = true, Message = "Spinned", Data = lastHistoryItem }, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult Spin()
        {
            var spinResult = slotEngine.Spin();

            if(spinResult == null)
            {
                return Json(new { OK = false, Message = "Not enough balance!", Data = spinResult }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { OK = true, Message = "Spinned", Data = spinResult }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}