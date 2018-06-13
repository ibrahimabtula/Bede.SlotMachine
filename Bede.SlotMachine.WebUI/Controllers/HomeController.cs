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

        [HttpGet]
        public JsonResult GetBalance()
        {
            var balance = slotEngine.GetBalance();

            return Json(new { OK = true, Message = "Balance", Data = balance }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLastSpin()
        {
            var last = slotEngine.SpinHistory.LastOrDefault();

            if(last != null)
            {
                return Json(new { OK = true, Message = "", Data = last }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { OK = false, Message = "No previous plays, enter deposit and stake to start new game", Data = last }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult StartAgain()
        {
            slotEngine.Reset();
            return Json(new { OK = true, Message = "Welcome to this slot game" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EnterStake(double? stake)
        {
            if (!stake.HasValue)
            {
                return Json(new { OK = false, Message = "Value is NULL!" }, JsonRequestBehavior.AllowGet);
            }

            slotEngine.Stake = stake.Value;

            return Json(new { OK = true, Message = "Stake entered" }, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public JsonResult Spin()
        {
            var result = slotEngine.Spin();

            return Json(new { OK = result.success, Message = result.message, Data = result.spin }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}