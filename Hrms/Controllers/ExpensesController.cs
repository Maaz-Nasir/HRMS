using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hrms.Models;
using static Hrms.Helpers.ApplicationHelper;
using System.Linq.Dynamic.Core;

namespace Hrms.Controllers
{
    public class ExpensesController : BaseController
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
                var Data = (from x in dbContext.Expenses select x);
                if (!string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection))
                {
                    Data = Data.Where(x => !x.IsDeleted).OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.ExpenseFrom.Contains(searchValue)
                                                || m.ExpenseTo.Contains(searchValue)
                                                || Convert.ToString(m.ExpenseDate).Contains(searchValue)
                                                || m.ExpenseYear.Contains(searchValue)
                                                || m.ExpenseMonth.Contains(searchValue)
                                                || m.Amount.ToString().Contains(searchValue)
                                                || m.Organization.Name.Contains(searchValue)
                                                || m.ExpenseType.Name.Contains(searchValue)
                                                || m.Status.Contains(searchValue) && !m.IsDeleted);
                }
                recordsTotal = Data.Count();
                var resultList = Data.Skip(skip).Take(pageSize).ToList();
                var resultData = from x in resultList.Where(x => x.SubscriptionId == SessionData.SubscriptionId && !x.IsDeleted)
                                 select new { x.Id, ExpenseType = x.ExpenseType.Name, Organization = x.Organization.Name, x.ExpenseFrom,
                                     x.ExpenseTo, ExpenseDate = Convert.ToDateTime(x.ExpenseDate).ToString("dd-MMM-yyyy"), x.ExpenseYear, 
                                     x.ExpenseMonth, x.Amount, x.Status };

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
            return View("Form", new Expenses());
        }
        public Expenses GetRecord(int? id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var Record = dbContext.Expenses.FirstOrDefault(o => o.Id == id && o.SubscriptionId == UserRecord.SubscriptionId && o.IsDeleted == false);
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
                return Redirect(ViewBag.WebsiteURL + "expenses/add");
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
                return Redirect(ViewBag.WebsiteURL + "expenses/add");
            }
        }

        [HttpPost]
        public IActionResult Save(Expenses model)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            UserSessionData SessionData = GetUserData(this);
            try
            {
                if (IsUserLogin(this))
                {
                    bool isAddOrUpdate = true;
                    if (isAddOrUpdate)
                    {
                        model.ExpenseYear = model.ExpenseDate.Year.ToString();
                        model.ExpenseMonth = model.ExpenseDate.ToString("MMMM");
                        model.SubscriptionId = SessionData.SubscriptionId;
                        if (model.Id == 0)
                        {
                            model.CreatedBy = SessionData.UserId;
                            model.CreatedDateTime = GetDateTime(SessionData.SubscriptionTimeZone);
                            model.UtcCreatedDateTime = GetUtcDateTime();
                            dbContext.Expenses.Add(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Saved Successfully";
                        }
                        else
                        {
                            model.UpdatedBy = SessionData.UserId;
                            model.UpdatedDateTime = GetDateTime(SessionData.SubscriptionTimeZone);
                            model.UtcUpdatedDateTime = GetUtcDateTime();
                            dbContext.Expenses.Update(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Updated Successfully";
                        }
                        dbContext.SaveChanges();
                        ajaxResponse.TargetURL = ViewBag.WebsiteURL + "expenses";
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
                        var RecordToDelete = dbContext.Expenses.FirstOrDefault(o => o.Id == Id && o.SubscriptionId == UserRecord.SubscriptionId && o.OrganizationId == UserRecord.OrganizationId);
                        if (RecordToDelete != null)
                        {
                            RecordToDelete.IsDeleted = true;
                            RecordToDelete.DeletedBy = UserRecord.UserId;
                            RecordToDelete.DeletedDate = GetDateTime(UserRecord.SubscriptionTimeZone);
                            dbContext.Expenses.Update(RecordToDelete);
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
