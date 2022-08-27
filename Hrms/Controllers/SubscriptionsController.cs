using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Hrms.Helpers.ApplicationHelper;
using System.Linq.Dynamic.Core;
using Hrms.Models;

namespace Hrms.Controllers
{
    public class SubscriptionsController : BaseController
    {
        [HttpPost]
        public IActionResult Listener()
        {
            try
            {
                UserSessionData SessionData = GetUserData(this);
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var Data = (from x in dbContext.Subscriptions select x);
                if (!string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection))
                {
                    Data = Data.Where(x => !x.IsDeleted).OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.Title.Contains(searchValue)
                                                || m.Code.Contains(searchValue)
                                                || m.Country.Name.Contains(searchValue)
                                                || m.TimeZoneId.Contains(searchValue)
                                                || Convert.ToString(m.StartDate).Contains(searchValue)
                                                || Convert.ToString(m.EndDate).Contains(searchValue)
                                                || Convert.ToString(m.NoOfMonth).Contains(searchValue)
                                                || m.Status.Contains(searchValue) && !m.IsDeleted);
                }
                recordsTotal = Data.Count();
                var resultList = Data.Skip(skip).Take(pageSize).ToList();
                var resultData = from x in resultList.Where(x => !x.IsDeleted)
                                 select new { x.Id, Country = x.Country.Name, x.TimeZoneId, x.Code, x.Title, StartDate = Convert.ToDateTime(x.StartDate).ToString("dd-MMM-yyyy"), EndDate = Convert.ToDateTime(x.EndDate).ToString("dd-MMM-yyyy"), x.NoOfMonth, x.Status };

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resultData };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult Index()
        {
            ViewBag.PageType = EnumPageType.Index;
            return View();
        }
        public IActionResult Add()
        {
            UserSessionData UserRecord = GetUserData(this);
            DropdownForHRMS(this);
            ViewBag.PageType = EnumPageType.Add;
            return View("Form", new Subscriptions() { StartDate = GetDateTime(UserRecord.SubscriptionTimeZone), EndDate = GetDateTime(UserRecord.SubscriptionTimeZone) });
        }
        public Subscriptions GetRecord(int? id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var Record = dbContext.Subscriptions.FirstOrDefault(o => o.Id == id && o.IsDeleted == false);
            if (Record != null)
            {
                DropdownForHRMS(this);
            }
            return Record;
        }
        public IActionResult Edit(int? id)
        {
            var Record = GetRecord(id);
            if (Record != null)
            {
                ViewBag.PageType = EnumPageType.Edit;
                return View("Form", Record);
            }
            else
            {
                return Redirect(ViewBag.WebsiteURL + "subscriptions/add");
            }
        }
        public IActionResult View(int? id)
        {
            var Record = GetRecord(id);
            if (Record != null)
            {
                ViewBag.PageType = EnumPageType.View;
                return View("Form", Record);
            }
            else
            {
                return Redirect(ViewBag.WebsiteURL + "subscriptions/add");
            }
        }

        [HttpPost]
        public IActionResult Save(Subscriptions model)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            UserSessionData SessionData = GetUserData(this);
            try
            {
                if (IsUserLogin(this))
                {
                    bool isAddOrUpdate = true;
                    if (model.Code.ToLower().Equals("portal"))
                    {
                        ajaxResponse.Message = "Portal is reserved word";
                        isAddOrUpdate = false;
                    }
                    Subscriptions subscriptionRecord = dbContext.Subscriptions.FirstOrDefault(o => o.Id != model.Id && o.Code.ToLower().Equals(model.Code.ToLower()) && o.IsDeleted == false);
                    if (isAddOrUpdate)
                    {
                        if (subscriptionRecord != null)
                        {
                            ajaxResponse.Message = "Subscripion code already exist in our records";
                            isAddOrUpdate = false;
                        }
                    }
                    UserAccounts modelUser = new UserAccounts()
                    {
                        Id = model.UserId,
                        Username = model.Username,
                        Password = model.Password,
                        Email = model.EmailAddress
                    };
                    model.UserAccounts = null;
                    UserAccounts Record = dbContext.UserAccounts.FirstOrDefault(o => o.Id != modelUser.Id && o.Username.ToLower().Equals(modelUser.Username.ToLower()) && o.IsDeleted == false);
                    if (isAddOrUpdate)
                    {
                        if (Record != null)
                        {
                            ajaxResponse.Message = "Username already exist in our records";
                            isAddOrUpdate = false;
                        }
                    }
                    if (isAddOrUpdate)
                    {
                        UserAccounts Record1 = dbContext.UserAccounts.FirstOrDefault(o => o.Id != modelUser.Id && o.Email.ToLower().Equals(modelUser.Email.ToLower()) && o.IsDeleted == false);
                        if (Record1 != null)
                        {
                            ajaxResponse.Message = "Email Address already exist in our records";
                            isAddOrUpdate = false;
                        }
                    }
                    if (isAddOrUpdate)
                    {
                        List<RolePermissions> rolePermissionList = new List<RolePermissions>();
                        List<int> menuIds = new List<int>();
                        List<SubscriptionModules> subscriptionModules = model.SubscriptionModules.Where(x => x.SubscriptionId == 1).ToList();
                        int rpPosition = 0;
                        foreach (var rec in subscriptionModules)
                        {
                            RolePermissions rp = new RolePermissions();
                            rp.MenuId = rec.MenuId;
                            rp.Permission = "All";
                            rp.SequenceOrder = rpPosition;
                            rp.Type = EnumMenuType.Children;
                            rolePermissionList.Add(rp);
                            rpPosition += 1;
                            menuIds.Add(rec.MenuId);
                        }
                        var ParentMenuIds = dbContext.Menus.Where(o => menuIds.Contains(o.Id)).OrderBy(o => o.Parent.SequenceOrder).Select(s => s.Parent.Id).Distinct().ToList();
                        rpPosition = 0;
                        foreach (int rec in ParentMenuIds)
                        {
                            RolePermissions rp = new RolePermissions();
                            rp.MenuId = rec;
                            rp.Permission = "All";
                            rp.SequenceOrder = rpPosition;
                            rp.Type = EnumMenuType.Parent;
                            rolePermissionList.Add(rp);
                            rpPosition += 1;
                        }
                        model.SubscriptionModules = new List<SubscriptionModules>();
                        model.EndDate = model.StartDate.AddMonths(model.NoOfMonth);
                        if (model.EndDate <= GetUtcDateTime())
                        {
                            model.Status = EnumEnableDisable.Disable;
                        }
                        else
                        {
                            model.Status = EnumEnableDisable.Enable;
                        }
                        if (model.Id == 0)
                        {
                            model.CreatedBy = SessionData.UserId;
                            model.CreatedDateTime = GetDateTime(SessionData.SubscriptionTimeZone);
                            model.UtcCreatedDateTime = GetUtcDateTime();
                            dbContext.Subscriptions.Add(model);
                            Roles r = new Roles()
                            {
                                SubscriptionId = 1,
                                Name = EnumRole.Administrator,
                                CreatedDateTime = GetUtcDateTime(),
                                UtcCreatedDateTime = GetUtcDateTime(),
                                CreatedBy = SessionData.UserId
                            };
                            dbContext.Roles.Add(r);
                            rolePermissionList.ForEach(x => x.Role = r);
                            UserAccounts u = new UserAccounts()
                            {
                                SubscriptionId = 1,
                                Role = r,
                                Name = modelUser.Username,
                                Username = modelUser.Username,
                                Password = Encrypt(modelUser.Password),
                                Email = modelUser.Email,
                                Phone = model.ContactNo.ToString(),
                                Status = EnumEnableDisable.Enable,
                                CreatedDateTime = GetUtcDateTime(),
                                UtcCreatedDateTime = GetUtcDateTime(),
                                CreatedBy = SessionData.UserId
                            };
                            dbContext.UserAccounts.Add(u);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Saved Successfully";
                        }
                        else
                        {
                            List<SubscriptionModules> subscriptionModulesRecords = dbContext.SubscriptionModules.Where(x => x.SubscriptionId == model.Id).ToList();
                            dbContext.SubscriptionModules.RemoveRange(subscriptionModulesRecords);
                            model.UpdatedDateTime = GetUtcDateTime();
                            model.UpdatedBy = SessionData.UserId;
                            dbContext.Subscriptions.Update(model);
                            var userRecordToUpdate = dbContext.UserAccounts.FirstOrDefault(x => x.Id == modelUser.Id);
                            userRecordToUpdate.Username = modelUser.Username;
                            if (!string.IsNullOrWhiteSpace(modelUser.Password))
                            {
                                userRecordToUpdate.Password = Encrypt(modelUser.Password);
                            }
                            userRecordToUpdate.Email = modelUser.Email;
                            userRecordToUpdate.UpdatedDateTime = GetUtcDateTime();
                            userRecordToUpdate.UtcUpdatedDateTime = GetUtcDateTime();
                            userRecordToUpdate.UpdatedBy = SessionData.UserId;
                            dbContext.UserAccounts.Update(userRecordToUpdate);
                            List<RolePermissions> rolePermissionRecords = dbContext.RolePermissions.Where(x => x.RoleId == userRecordToUpdate.RoleId).ToList();
                            dbContext.RolePermissions.RemoveRange(rolePermissionRecords);
                            rolePermissionList.ForEach(x => x.RoleId = userRecordToUpdate.RoleId);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Updated Successfully";
                        }
                        subscriptionModules.ForEach(x => x.SubscriptionId = model.Id);
                        dbContext.SubscriptionModules.AddRange(subscriptionModules);
                        dbContext.RolePermissions.AddRange(rolePermissionList);
                        dbContext.SaveChanges();
                        ajaxResponse.TargetURL = ViewBag.WebsiteURL + "subscriptions";
                    }
                }
                else
                {
                    ajaxResponse.Message = "Session Expired";
                    ajaxResponse.TargetURL = ViewBag.WebsiteURL;
                }
            }
            catch (Exception ex)
            {
                ajaxResponse.Success = false;
                ajaxResponse.Message = ex.Message;
            }
            return Json(ajaxResponse);
        }
        [HttpPost]
        public JsonResult Delete(int? Id)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Message = "Data not found in our records";
            try
            {
                if (IsUserLogin(this))
                {
                    if (Id == 0)
                    {
                        ajaxResponse.Message = "ID is not in numeric format";
                    }
                    else
                    {
                        UserSessionData UserRecord = GetUserData(this);
                        var RecordToDelete = dbContext.Subscriptions.FirstOrDefault(o => o.Id == Id);
                        if (RecordToDelete != null)
                        {
                            RecordToDelete.IsDeleted = true;
                            RecordToDelete.DeletedBy = UserRecord.UserId;
                            RecordToDelete.DeletedDate = GetDateTime(UserRecord.SubscriptionTimeZone);
                            dbContext.Subscriptions.Update(RecordToDelete);
                            dbContext.SaveChanges();
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Deleted Successfully";
                        }
                    }
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
    }
}
