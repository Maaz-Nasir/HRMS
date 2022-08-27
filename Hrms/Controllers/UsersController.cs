﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hrms.Models;
using static Hrms.Helpers.ApplicationHelper;
using System.Linq.Dynamic.Core;

namespace Hrms.Controllers
{
    public class UsersController : BaseController
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
                var Data = (from x in dbContext.UserAccounts select x);
                if (!string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection))
                {
                    Data = Data.Where(x => !x.IsDeleted).OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.Name.Contains(searchValue)
                                                || m.Username.Contains(searchValue)
                                                || m.Email.Contains(searchValue)
                                                || m.Phone.Contains(searchValue)
                                                || m.Role.Name.Contains(searchValue)
                                                || m.Status.Contains(searchValue) && !m.IsDeleted);
                }
                recordsTotal = Data.Count();
                var resultList = Data.Skip(skip).Take(pageSize).ToList();
                var resultData = from x in resultList.Where(x => x.SubscriptionId == SessionData.SubscriptionId && !x.IsDeleted)
                                 select new { x.Id, Role = x.Role.Name, x.Name, x.Username, x.Email, x.Status };

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
            ViewBag.PageType = EnumPageType.Add;
            DropdownForHRMS(this);
            return View("Form", new UserAccounts());
        }
        public UserAccounts GetRecord(int? id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var Record = dbContext.UserAccounts.FirstOrDefault(o => o.Id == id && o.SubscriptionId == UserRecord.SubscriptionId && o.IsDeleted == false);
            if (Record != null)
            {
                Record.Password = Decrypt(Record.Password);
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
                return Redirect(ViewBag.WebsiteURL + "users/add");
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
                return Redirect(ViewBag.WebsiteURL + "users/add");
            }
        }

        [HttpPost]
        public IActionResult Save(UserAccounts model)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            UserSessionData SessionData = GetUserData(this);
            try
            {
                if (IsUserLogin(this))
                {
                    bool isAddOrUpdate = true;
                    var record = dbContext.UserAccounts.FirstOrDefault(x => x.Id != model.Id && x.Email.ToLower().Equals(model.Email.ToLower()) && x.OrganizationId == SessionData.OrganizationId && x.SubscriptionId == SessionData.SubscriptionId && !x.IsDeleted && x.Status == EnumEmployeeStatus.Active);
                    if (record != null)
                    {
                        isAddOrUpdate = false;
                        ajaxResponse.Message = "Record Already Exists";
                    }
                    if (isAddOrUpdate)
                    {
                        model.SubscriptionId = SessionData.SubscriptionId;
                        if (model.Id == 0)
                        {
                            model.CreatedBy = SessionData.UserId;
                            model.CreatedDateTime = GetDateTime(SessionData.SubscriptionTimeZone);
                            model.UtcCreatedDateTime = GetUtcDateTime();
                            model.Password = Encrypt(model.Password);
                            dbContext.UserAccounts.Add(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Saved Successfully";
                        }
                        else
                        {
                            model.UpdatedBy = SessionData.UserId;
                            model.UpdatedDateTime = GetDateTime(SessionData.SubscriptionTimeZone);
                            model.UtcUpdatedDateTime = GetUtcDateTime();
                            model.Password = Encrypt(model.Password);
                            dbContext.UserAccounts.Update(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Updated Successfully";
                        }
                        dbContext.SaveChanges();
                        ajaxResponse.TargetURL = ViewBag.WebsiteURL + "users";
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
                        var RecordToDelete = dbContext.UserAccounts.FirstOrDefault(o => o.Id == Id && o.SubscriptionId == UserRecord.SubscriptionId && o.OrganizationId == UserRecord.OrganizationId);
                        if (RecordToDelete != null)
                        {
                            if (RecordToDelete.Id == UserRecord.UserId)
                            {
                                ajaxResponse.Message = "Unable to delete this record because of some reference data";
                            }
                            else
                            {
                                RecordToDelete.IsDeleted = true;
                                RecordToDelete.DeletedBy = UserRecord.UserId;
                                RecordToDelete.DeletedDate = GetDateTime(UserRecord.SubscriptionTimeZone);
                                dbContext.UserAccounts.Update(RecordToDelete);
                                dbContext.SaveChanges();
                                ajaxResponse.Success = true;
                                ajaxResponse.Message = "Record Deleted Successfully";
                            }    
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