using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hrms.Models;
using static Hrms.Helpers.ApplicationHelper;
using System.Linq.Dynamic.Core;
using Newtonsoft.Json;

namespace Hrms.Controllers
{
    public class MenusController : BaseController
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
                var Data = (from x in dbContext.Menus select x);
                if (!string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection))
                {
                    Data = Data.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.Name.Contains(searchValue)
                                                || m.ParentId.ToString().Contains(searchValue)
                                                || m.AccessUrl.ToString().Contains(searchValue)
                                                || m.Icon.ToString().Contains(searchValue)
                                                || m.Type.ToString().Contains(searchValue)
                                                || m.SequenceOrder.ToString().Contains(searchValue)
                                                || m.CreatedDateTime.ToString().Contains(searchValue));
                }
                recordsTotal = Data.Count();
                var resultList = Data.Skip(skip).Take(pageSize).ToList();
                var resultData = from x in resultList
                                 select new { x.Id, Parent = x.Parent != null ? x.Parent.Name : "#", x.Name, x.Type, x.AccessUrl, x.Icon, x.SequenceOrder,
                                     CreatedDateTime = Convert.ToDateTime(x.CreatedDateTime).ToString("dd-MMM-yyyy : hh:mm:ss")
                                 };

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = resultData };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult MenuPermissionListener(int id)
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
                var Data = (from x in dbContext.MenuPermissions select x);
                if (!string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection))
                {
                    Data = Data.Where(x => x.MenuId == id).OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.Name.Contains(searchValue)
                                                || m.MenuId.ToString().Contains(searchValue)
                                                || m.Ptype.ToString().Contains(searchValue)
                                                || m.Type.ToString().Contains(searchValue)
                                                || m.SequenceOrder.ToString().Contains(searchValue)
                                                || m.CreatedDateTime.ToString().Contains(searchValue));
                }
                recordsTotal = Data.Count();
                var resultList = Data.Skip(skip).Take(pageSize).ToList();
                var resultData = from x in resultList
                                 select new
                                 {
                                     x.Id,
                                     Menu = x.Menu.Name,
                                     x.Name,
                                     x.Type,
                                     x.Ptype,
                                     x.SequenceOrder,
                                     CreatedDateTime = Convert.ToDateTime(x.CreatedDateTime).ToString("dd-MMM-yyyy : hh:mm:ss")
                                 };

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
            return View("Form", new Menus());
        }
        public Menus GetRecord(int? id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var Record = dbContext.Menus.FirstOrDefault(o => o.Id == id);
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
                return Redirect(ViewBag.WebsiteURL + "menus/add");
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
                return Redirect(ViewBag.WebsiteURL + "menus/add");
            }
        }

        [HttpPost]
        public IActionResult Save(Menus model)
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
                        if (model.Id == 0)
                        {
                            model.CreatedBy = SessionData.UserId;
                            model.CreatedDateTime = GetDateTime(SessionData.SubscriptionTimeZone);
                            model.UtcCreatedDateTime = GetUtcDateTime();
                            dbContext.Menus.Add(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Saved Successfully";
                        }
                        else
                        {
                            model.UpdatedBy = SessionData.UserId;
                            dbContext.Menus.Update(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Updated Successfully";
                        }
                        dbContext.SaveChanges();
                        ajaxResponse.TargetURL = ViewBag.WebsiteURL + "menus";
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
        public IActionResult SaveMenuPermission(MenuPermissions model)
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
                        if (model.Id == 0)
                        {
                            model.CreatedBy = SessionData.UserId;
                            model.CreatedDateTime = GetDateTime(SessionData.SubscriptionTimeZone);
                            model.UtcCreatedDateTime = GetUtcDateTime();
                            dbContext.MenuPermissions.Add(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Saved Successfully";
                        }
                        else
                        {
                            model.UpdatedBy = SessionData.UserId;
                            dbContext.MenuPermissions.Update(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Updated Successfully";
                        }
                        dbContext.SaveChanges();
                        ajaxResponse.TargetURL = ViewBag.WebsiteURL + "menus";
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
        public JsonResult GetMenuPermissionRecord(int? id)
        {
            var Record = dbContext.MenuPermissions.Select(x => new { x.Id, x.MenuId, x.Name, x.Ptype, x.Type, x.SequenceOrder, x.CreatedDateTime, x.UtcCreatedDateTime, x.CreatedBy }).FirstOrDefault(o => o.Id == id);
            return Json(Record);
        }
    }
}
