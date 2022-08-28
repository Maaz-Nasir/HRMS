using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hrms.Models;
using static Hrms.Helpers.ApplicationHelper;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Hrms.Controllers
{
    public class OrganizationsController : BaseController
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
                var Data = (from x in dbContext.Organizations select x);
                if (!string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection))
                {
                    Data = Data.Where(x => !x.IsDeleted).OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.Name.Contains(searchValue)
                                                || m.Code.Contains(searchValue)
                                                || m.CreatedDateTime.ToString().Contains(searchValue)
                                                || m.UpdatedDateTime.ToString().Contains(searchValue)
                                                || m.Status.Contains(searchValue) && !m.IsDeleted);
                }
                recordsTotal = Data.Count();
                var resultList = Data.Skip(skip).Take(pageSize).ToList();
                var resultData = from x in resultList.Where(x => x.SubscriptionId == SessionData.SubscriptionId && !x.IsDeleted)
                                 select new
                                 {
                                     x.Id,
                                     x.Logo,
                                     x.Code,
                                     x.Name,
                                     x.Status,
                                     CreatedDateTime = Convert.ToDateTime(x.CreatedDateTime).ToString("dd-MMM-yyyy : hh:mm:ss"),
                                     UpdatedDateTime = x.UpdatedDateTime == null ? "" : Convert.ToDateTime(x.UpdatedDateTime).ToString("dd-MMM-yyyy : hh:mm:ss")
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
            return View("Form", new Organizations());
        }
        public Organizations GetRecord(int? id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var Record = dbContext.Organizations.FirstOrDefault(o => o.Id == id && o.SubscriptionId == UserRecord.SubscriptionId && o.IsDeleted == false);
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
                return Redirect(ViewBag.WebsiteURL + "organizations/add");
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
                return Redirect(ViewBag.WebsiteURL + "organizations/add");
            }
        }

        [HttpPost]
        public IActionResult Save(Organizations model, IFormFile Image)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            UserSessionData UserRecord = GetUserData(this);
            try
            {
                if (IsUserLogin(this))
                {
                    bool isAddOrUpdate = true;
                    if (isAddOrUpdate)
                    {
                        string FilePath = "";
                        if (Image != null)
                        {
                            string fileName = Image.FileName;
                            string path = Path.Combine(ViewBag.WebRootPath, OrganizationLogoFilePath, fileName);
                            if (System.IO.File.Exists(path))
                            {
                                fileName = CreateFileName(Path.GetFileNameWithoutExtension(fileName)) + Path.GetExtension(fileName).ToLower();
                                path = Path.Combine(ViewBag.WebRootPath, OrganizationLogoFilePath, fileName);
                            }
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                Image.CopyTo(stream);
                            }
                            FilePath = OrganizationLogoFilePath + "/" + fileName;
                        }
                        model.SubscriptionId = UserRecord.SubscriptionId;
                        if (model.Id == 0)
                        {
                            model.CreatedBy = UserRecord.UserId;
                            model.CreatedDateTime = GetDateTime(UserRecord.SubscriptionTimeZone);
                            model.UtcCreatedDateTime = GetUtcDateTime();
                            if (FilePath != "")
                            {
                                model.Logo = FilePath;
                            }
                            dbContext.Organizations.Add(model);
                            List<string> roleNames = new List<string>()
                            {
                                EnumOrganizationRoles.Admin,
                                EnumOrganizationRoles.Employee
                            };
                            foreach (var property in roleNames)
                            {
                                Roles role = new Roles();
                                role.Name = property.ToString();
                                role.SubscriptionId = UserRecord.SubscriptionId;
                                role.OrganizationId = model.Id;
                                role.CreatedBy = UserRecord.UserId;
                                role.CreatedDateTime = GetDateTime(UserRecord.SubscriptionTimeZone);
                                role.UtcCreatedDateTime = GetUtcDateTime();
                                role.IsEditable = true;
                                dbContext.Roles.Add(role);
                            }
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Saved Successfully";
                        }
                        else
                        {
                            model.UpdatedBy = UserRecord.UserId;
                            model.UpdatedDateTime = GetDateTime(UserRecord.SubscriptionTimeZone);
                            model.UtcUpdatedDateTime = GetUtcDateTime();
                            if (FilePath != "")
                            {
                                model.Logo = FilePath;
                            }
                            dbContext.Organizations.Update(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Updated Successfully";
                        }
                        dbContext.SaveChanges();
                        ajaxResponse.TargetURL = ViewBag.WebsiteURL + "organizations";
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
                        var RecordToDelete = dbContext.Organizations.FirstOrDefault(o => o.Id == Id && o.SubscriptionId == UserRecord.SubscriptionId);
                        if (RecordToDelete != null)
                        {
                            //if (RecordToDelete.UserAccounts.Count > 0)
                            //{
                            //    ajaxResponse.Message = "Unable to delete this record because of some reference data";
                            //}
                            //else
                            //{
                            //    RecordToDelete.IsDeleted = true;
                            //    RecordToDelete.DeletedBy = UserRecord.UserId;
                            //    RecordToDelete.DeletedDate = GetDateTime(UserRecord.SubscriptionTimeZone);
                            //    dbContext.Organizations.Update(RecordToDelete);
                            //    dbContext.SaveChanges();
                            //    ajaxResponse.Success = true;
                            //    ajaxResponse.Message = "Record Deleted Successfully";
                            //}
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
