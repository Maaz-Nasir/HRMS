using Hrms.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Hrms.Helpers.ApplicationHelper;

namespace Hrms.Controllers
{
    public class LoginController : Controller
    {
        HrmsContext dbContext;
        public LoginController()
        {
            dbContext = new HrmsContext();
        }

        public IActionResult Index()
        {
            ViewBag.PageType = EnumPageType.Index;
            if (IsUserLogin(this))
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ViewBag.WebsiteName = GetSettingContentByName(dbContext, "Website Name");
                return View(new UserAccounts());
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserLogin(UserAccounts user, string RememberMe)
        {
            var host = Request.Host.Host.Split(".").FirstOrDefault();
            AjaxResponse ajaxResponse = new AjaxResponse();
            try
            {
                user.Password = Encrypt(user.Password);
                var userRecord = dbContext.UserAccounts.FirstOrDefault(x => x.Username.ToLower().Equals(user.Username.ToLower()) && x.Password.Equals(user.Password) && x.Status.Equals(EnumEnableDisable.Enable) && x.IsDeleted == false && (x.SubscriptionId == null || (GetUtcDateTime().Date <= x.Subscription.StartDate.AddMonths(x.Subscription.NoOfMonth) && !x.Subscription.IsDeleted)));
                if (userRecord == null)
                {
                    throw new Exception("Email Address or Password incorrect");
                }
                else
                {
                    var userSessionData = new UserSessionData();
                    userSessionData.UserId = userRecord.Id;
                    userSessionData.SubscriptionId = userRecord.SubscriptionId;
                    userSessionData.SubscriptionCountryId = userRecord.Subscription.Country.Id;
                    userSessionData.RoleId = userRecord.RoleId;
                    userSessionData.RoleName = userRecord.Role.Name;
                    userSessionData.SubscriptionCountryName = userRecord.Subscription.Country.Name;
                    userSessionData.SubscriptionCode = userRecord.Subscription.Code;
                    userSessionData.SubscriptionTimeZone = userRecord.Subscription.TimeZoneId;
                    userSessionData.Name = userRecord.Name;
                    userSessionData.Username = userRecord.Username;
                    userSessionData.EmailAddress = userRecord.Email;
                    userSessionData.LoginCode = userRecord.Username;
                    userSessionData.SubDomain = userRecord.Subscription.Title;
                    userSessionData.ProfileImage = "Not";
                    userSessionData.ProfileImageWithPath = "Not Find";
                    userSessionData.EmployeeId = 1;
                    userSessionData.OrganizationId = userRecord.OrganizationId == null ? 0 : userRecord.OrganizationId;
                    AddSession(this, SessionUserLogin, JsonConvert.SerializeObject(userSessionData));
                    ajaxResponse.Success = true;
                    ajaxResponse.Message = "Logged in successfully";
                }
            }
            catch (Exception e)
            {
                ajaxResponse.Success = false;
                ajaxResponse.Message = e.Message;
            }
            return Json(ajaxResponse);
        }
    }
}
