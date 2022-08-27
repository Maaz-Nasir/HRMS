using Hrms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using static Hrms.Helpers.ApplicationHelper;

namespace Hrms.Controllers
{
    //[ActionFilter]
    public class BaseController : Controller
    {
        protected HrmsContext dbContext;
        public BaseController()
        {
            dbContext = new HrmsContext();
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var host = Request.Host.Host.Split(".").FirstOrDefault();
            Controller controllerContext = (Controller)filterContext.Controller;
            UserSessionData UserRecord = GetUserData(controllerContext);
            if (UserRecord != null)
            {
                ViewBag.WebRootPath = Startup.CurrentHostingEnvironment.WebRootPath;
                ViewBag.contentRootPath = Startup.CurrentHostingEnvironment.ContentRootPath;
                ViewBag.WebsiteName = GetSettingContentByName(dbContext, "Website Name");
                ViewBag.WebsiteURL = GetSettingContentByName(dbContext, "Website URL");
                bool isAjaxCall = filterContext.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest";
                if (!isAjaxCall)
                {
                    string[] requestURL = filterContext.HttpContext.Request.Path.ToString().Split('/');
                    string controllerURL = requestURL[1].ToLower();
                    if (!IsUserLogin(controllerContext))
                    {
                        filterContext.Result = new RedirectResult("/login");
                    }
                    else
                    {
                        ViewBag.ControllerName = UpperCaseWords(controllerURL);
                        ViewBag.ControllerURL = controllerURL;
                        string menuURL = controllerURL;
                        string actionURL = string.Empty;
                        if (requestURL.Length > 2)
                        {
                            actionURL = requestURL[2].ToLower();
                            if (actionURL != "add" && actionURL != "edit" && actionURL != "import" && actionURL != "view" && actionURL != "sorting" && actionURL != "metas" && actionURL != "approval" && actionURL != "approve" && actionURL != "viewsurveysummary" && actionURL != "viewsurveytemplate" && actionURL != "balloonpayment" && actionURL != "review")
                            {
                                menuURL += "/" + actionURL;
                            }
                        }
                        //List<int> subscriptionModules = dbContext.SubscriptionModules.Where(x => x.SubscriptionId == UserRecord.SubscriptionId).Select(s => s.MenuId).ToList();
                        //var UserRolePermissionRecords = dbContext.RolePermissions.Where(o => o.RoleId == UserRecord.RoleId && (o.Role.Name.Equals(EnumRole.SuperAdministrator) || (subscriptionModules.Contains(o.Menu.Id) || o.Menu.Parent == null))).ToList();
                        //if (!AllowedLink().Contains(menuURL))
                        //{
                        //    filterContext.Result = new RedirectResult("/dashboard/accessunauthorized");
                        //    if (UserRolePermissionRecords.Count > 0)
                        //    {
                        //        var UserRolePermissionRecord = UserRolePermissionRecords.FirstOrDefault(o => o.Menu.AccessUrl.ToLower().Equals(menuURL));
                        //        if (UserRolePermissionRecord != null)
                        //        {
                        //            if (UserRolePermissionRecord.Permission.ToLower().Equals("all"))
                        //            {
                        //                filterContext.Result = null;
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("add")))
                        //                {
                        //                    ViewBag.AllowAdding = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("import")))
                        //                {
                        //                    ViewBag.AllowImporting = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("edit")))
                        //                {
                        //                    ViewBag.AllowEditing = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("delete")))
                        //                {
                        //                    ViewBag.AllowDeleting = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("view")))
                        //                {
                        //                    ViewBag.AllowViewing = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("sorting")))
                        //                {
                        //                    ViewBag.AllowSorting = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("metas")))
                        //                {
                        //                    ViewBag.AllowMetas = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("approval")))
                        //                {
                        //                    ViewBag.AllowApproval = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("approve")))
                        //                {
                        //                    ViewBag.AllowApproval = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("view departmental survey")))
                        //                {
                        //                    ViewBag.AllowViewingDepartmentalSurvey = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("view survey template")))
                        //                {
                        //                    ViewBag.AllowViewingSurveyTemplate = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("review application")))
                        //                {
                        //                    ViewBag.AllowViewingVacancyApplication = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("balloonpayment")))
                        //                {
                        //                    ViewBag.AllowBalloonPayment = true;
                        //                }
                        //                if (UserRolePermissionRecord.Menu.MenuPermissions.Any(o => o.Name.ToLower().Equals("refresh")))
                        //                {
                        //                    ViewBag.AllowRefresh = true;
                        //                }
                        //            }
                        //            else
                        //            {
                        //                string[] UserPermissionArray = UserRolePermissionRecord.Permission.ToLower().Split('|');
                        //                if (string.IsNullOrWhiteSpace(actionURL) || UserPermissionArray.Contains(actionURL))
                        //                {
                        //                    filterContext.Result = null;
                        //                }
                        //                if (filterContext.Result == null)
                        //                {
                        //                    if (UserPermissionArray.Contains("add"))
                        //                    {
                        //                        ViewBag.AllowAdding = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("add"))
                        //                    {
                        //                        ViewBag.AllowImporting = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("edit"))
                        //                    {
                        //                        ViewBag.AllowEditing = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("delete"))
                        //                    {
                        //                        ViewBag.AllowDeleting = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("view"))
                        //                    {
                        //                        ViewBag.AllowViewing = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("sorting"))
                        //                    {
                        //                        ViewBag.AllowSorting = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("metas"))
                        //                    {
                        //                        ViewBag.AllowMetas = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("approval"))
                        //                    {
                        //                        ViewBag.AllowApproval = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("approve"))
                        //                    {
                        //                        ViewBag.AllowApproval = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("view departmental survey"))
                        //                    {
                        //                        ViewBag.AllowViewingDepartmentalSurvey = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("view survey template"))
                        //                    {
                        //                        ViewBag.AllowViewingSurveyTemplate = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("review application"))
                        //                    {
                        //                        ViewBag.AllowViewingVacancyApplication = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("balloonPayment"))
                        //                    {
                        //                        ViewBag.AllowBalloonPayment = true;
                        //                    }
                        //                    if (UserPermissionArray.Contains("refresh"))
                        //                    {
                        //                        ViewBag.AllowRefresh = true;
                        //                    }
                        //                }
                        //            }
                        //            if (filterContext.Result == null)
                        //            {
                        //                ViewBag.ControllerName = UserRolePermissionRecord.Menu.Name;
                        //            }
                        //        }
                        //    }
                        //}
                        if (filterContext.Result == null)
                        {
                            if (UserRecord.RoleName == EnumRole.SuperAdministrator)
                            {
                                ViewBag.SubscriptionRecords = dbContext.Subscriptions.Where(o => o.IsDeleted == false).ToList();
                            }
                            else if (UserRecord.RoleName == EnumRole.Administrator)
                            {
                                ViewBag.SubscriptionOrganizationRecords = dbContext.Organizations.Where(o => o.IsDeleted == false
                                && o.SubscriptionId == UserRecord.SubscriptionId).ToList();
                            }
                            else
                            {
                                ViewBag.EmployeeProfile = true;
                                ViewBag.ChangePassword = true;
                            }
                            ViewBag.UserRecord = UserRecord;
                            //if (UserRolePermissionRecords.Count > 0)
                            //{
                            //    ViewBag.UserRolePermissionRecords = UserRolePermissionRecords;
                            //}
                            //    if (!AllowedLink().Contains(menuURL))
                            //    {
                            //        //string BreadCrumbHtml = "<div class='breadcrumb'>";
                            //        //BreadCrumbHtml += "<h1 class='mr-2'>HRMS</h1>" + dbContext.Menus.Where(o => o.AccessUrl.ToLower().Equals(menuURL)).Select(o => o.Parent).FirstOrDefault().Name + "</h1>";
                            //        //BreadCrumbHtml += "<ul>";
                            //        //BreadCrumbHtml += "<li><a href='" + ConvertToWebURL(dbContext, "dashboard") + "'>Home</a></li>";
                            //        //var MenuPermissionList = dbContext.MenuPermissions.Select(o => o.Name.ToLower()).Distinct().ToList();
                            //        //if (MenuPermissionList.Contains(actionURL))
                            //        //{
                            //        //    BreadCrumbHtml += "<li><a href='" + ConvertToWebURL(dbContext, controllerURL) + "'>" + ViewBag.ControllerName + "</a></li>";
                            //        //    BreadCrumbHtml += "<li>" + UpperCaseFirstLetter(actionURL) + "</li>";
                            //        //}
                            //        //else
                            //        //{
                            //        //    BreadCrumbHtml += "<li>" + ViewBag.ControllerName + "</li>";
                            //        //}
                            //        //BreadCrumbHtml += "</ul>";
                            //        //BreadCrumbHtml += "</div>";
                            //        //BreadCrumbHtml += "<div class='separator-breadcrumb border-top'></div>";
                            //        //ViewBag.BreadCrumbHTML = BreadCrumbHtml;
                            //    }
                            //    else
                            //    {
                            //        if (!string.IsNullOrWhiteSpace(actionURL) && ViewBag.ControllerName != "Dashboard")
                            //        {
                            //            ViewBag.ControllerURL = actionURL;
                            //            ViewBag.ControllerName = UpperCaseFirstLetter(actionURL);
                            //        }
                            //        else if (ViewBag.ControllerName == "home")
                            //        {
                            //        }
                            //    }
                            }
                        }
                    ViewBag.PageURL = ViewBag.WebsiteURL + controllerURL;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/HttpHandler/HttpUnAuthorized");
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        protected override void Dispose(bool disposing)
        {
            dbContext.Dispose();
            base.Dispose(disposing);
        }
        public void DropdownForHRMS(Controller controller)
        {
            UserSessionData UserRecord = GetUserData(controller);
            if (UserRecord.RoleName == EnumRole.SuperAdministrator)
            {
                ViewBag.OrganizationRecords = dbContext.Organizations.Where(x => x.SubscriptionId == UserRecord.SubscriptionId && x.Status == EnumEnableDisable.Enable && !x.IsDeleted).ToList();
                ViewBag.RoleRecords = dbContext.Roles.Where(x => !x.IsDeleted).ToList();
                ViewBag.ExpenseTypeRecords = dbContext.ExpenseTypes.Where(x => x.Status == EnumEnableDisable.Enable && !x.IsDeleted).ToList();
                ViewBag.TimeZone = GetTimeZoneList();
                ViewBag.CountryRecords = dbContext.Countries.Where(o => o.IsDeleted == false).ToList();
            }
            if (UserRecord.RoleName == EnumRole.Administrator)
            {
                ViewBag.OrganizationRecords = dbContext.Organizations.Where(x => x.SubscriptionId == UserRecord.SubscriptionId && x.Status == EnumEnableDisable.Enable && !x.IsDeleted).ToList();
                ViewBag.RoleRecords = dbContext.Roles.Where(o => o.Name != EnumRole.Administrator && o.Name != EnumRole.Employee && o.SubscriptionId == UserRecord.SubscriptionId && o.IsDeleted == false).ToList();
                ViewBag.ExpenseTypeRecords = dbContext.ExpenseTypes.Where(x => x.SubscriptionId == UserRecord.SubscriptionId && x.Status == EnumEnableDisable.Enable && !x.IsDeleted).ToList();
                ViewBag.CountryRecords = dbContext.Countries.Where(o => o.IsDeleted == false).ToList();
                ViewBag.RegionRecords = dbContext.Regions.Where(o => o.SubscriptionId == UserRecord.SubscriptionId && o.IsDeleted == false).ToList();
                ViewBag.CityRecords = dbContext.Cities.Where(o => o.SubscriptionId == UserRecord.SubscriptionId && o.IsDeleted == false).ToList();
            }
            if (UserRecord.RoleName == EnumRole.Employee)
            {
                ViewBag.ExpenseTypeRecords = dbContext.ExpenseTypes.Where(x => x.SubscriptionId == UserRecord.SubscriptionId && x.OrganizationId == UserRecord.OrganizationId && x.Status == EnumEnableDisable.Enable && !x.IsDeleted).ToList();
            }
        }
    }
}
