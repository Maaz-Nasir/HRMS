using Hrms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static Hrms.Helpers.ApplicationHelper;
using Hrms.Extensions;
using Newtonsoft.Json;

namespace Hrms.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.PageType = EnumPageType.Index;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            RemoveSession(this, SessionUserLogin);
            return RedirectToAction("Index", "Login");
        }
        public JsonResult SetSubscription(int subscriberID)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "Post Data Not Found";
            try
            {
                if (IsUserLogin(this))
                {
                    var UserRecord = GetUserData(this);
                    Subscriptions SubcriptionRecord = dbContext.Subscriptions.FirstOrDefault(o => o.Id == subscriberID);
                    UserRecord.SubscriptionId = SubcriptionRecord.Id;
                    UserRecord.SubscriptionCountryId = SubcriptionRecord.CountryId;
                    UserRecord.SubscriptionCountryName = SubcriptionRecord.Country.Name;
                    UserRecord.SubscriptionCode = SubcriptionRecord.Code;
                    UserRecord.SubscriptionTimeZone = SubcriptionRecord.TimeZoneId;
                    AddSession(this, SessionUserLogin, JsonConvert.SerializeObject(UserRecord));
                    ajaxResponse.TargetURL = ViewBag.WebsiteURL + "home";
                    ajaxResponse.Message = "";
                    ajaxResponse.Success = true;
                }
                else
                {
                    ajaxResponse.Message = "Session Expired";
                    ajaxResponse.TargetURL = ViewBag.WebsiteURL;
                }
            }
            catch (Exception ex)
            {
                string _catchMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    _catchMessage += "<br/>" + ex.InnerException.Message;
                }
                ajaxResponse.Message = _catchMessage;
            }
            return Json(ajaxResponse);
        }
        public IActionResult Unauthorized()
        {
            ViewBag.WebsiteURL = GetSettingContentByName(dbContext, "Website URL");
            return View();
        }
    }
}
