using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Hrms.Models;
using static Hrms.Helpers.ApplicationHelper;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Hrms.Controllers
{
    public class EmployeesController : BaseController
    {
        [HttpGet]
        public IActionResult GetCities(int Id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var cities = dbContext.Cities.Where(x => x.CountryId == Id && x.SubscriptionId == UserRecord.SubscriptionId && x.Status == EnumEnableDisable.Enable && x.IsDeleted == false).Select(x => new { x.Id, x.Name }).ToList();
            return Json(cities);
        }
        public IActionResult GetDepartments(int Id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var departments = dbContext.Departments.Where(x => x.OrganizationId == Id && x.SubscriptionId == UserRecord.SubscriptionId && x.Status == EnumEnableDisable.Enable && x.IsDeleted == false).Select(x => new { x.Id, x.Name }).ToList();
            return Json(departments);
        }
        public IActionResult GetDesignations(int Id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var designations = dbContext.Designations.Where(x => x.OrganizationId == Id && x.SubscriptionId == UserRecord.SubscriptionId && x.Status == EnumEnableDisable.Enable && x.IsDeleted == false).Select(x => new { x.Id, x.Name }).ToList();
            return Json(designations);
        }
        [HttpPost]
        public IActionResult Listener()
        {
            try
            {
                UserSessionData UserRecord = GetUserData(this);
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var Data = (from x in dbContext.Employees select x);
                if (!string.IsNullOrEmpty(sortColumn) || string.IsNullOrEmpty(sortColumnDirection))
                {
                    Data = Data.Where(x => !x.IsDeleted).OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Data = Data.Where(m => m.Firstname.Contains(searchValue)
                                                || m.EmployeeCode.Contains(searchValue)
                                                || m.Lastname.Contains(searchValue)
                                                || m.Organization.Name.Contains(searchValue)
                                                || m.Country.Name.Contains(searchValue)
                                                || m.City.Name.Contains(searchValue)
                                                || m.Status.Contains(searchValue) && !m.IsDeleted);
                }
                recordsTotal = Data.Count();
                var resultList = Data.Skip(skip).Take(pageSize).ToList();
                var resultData = from x in resultList.Where(x => x.SubscriptionId == UserRecord.SubscriptionId && !x.IsDeleted)
                                 select new { x.Id, Organization = x.Organization.Name, Country = x.Country.Name, City = x.City.Name, x.EmployeeCode, x.Firstname, x.Lastname, x.Status };

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
            ViewBag.PageType = EnumPageType.Add;
            DropdownForHRMS(this);
            return View("Form", new Employees() { DateOfBirth = GetDateTime(UserRecord.SubscriptionTimeZone) });
        }
        public Employees GetRecord(int? id)
        {
            UserSessionData UserRecord = GetUserData(this);
            var Record = dbContext.Employees.FirstOrDefault(o => o.Id == id && o.SubscriptionId == UserRecord.SubscriptionId && o.IsDeleted == false);
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
                return Redirect(ViewBag.WebsiteURL + "employees/add");
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
                return Redirect(ViewBag.WebsiteURL + "employees/add");
            }
        }

        [HttpPost]
        public IActionResult Save(Employees model, IFormFile Image)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            UserSessionData UserRecord = GetUserData(this);
            try
            {
                if (IsUserLogin(this))
                {
                    bool isAddOrUpdate = true;
                    Organizations organizations = dbContext.Organizations.FirstOrDefault(s => s.Id == model.OrganizationId);
                    if (isAddOrUpdate)
                    {
                        string FilePath = "";
                        if (Image != null)
                        {
                            string fileName = Image.FileName;
                            string path = Path.Combine(ViewBag.WebRootPath, UserProfileImageFilePath, fileName);
                            if (System.IO.File.Exists(path))
                            {
                                fileName = CreateFileName(Path.GetFileNameWithoutExtension(fileName)) + Path.GetExtension(fileName).ToLower();
                                path = Path.Combine(ViewBag.WebRootPath, UserProfileImageFilePath, fileName);
                            }
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                Image.CopyTo(stream);
                            }
                            FilePath = UserProfileImageFilePath + "/" + fileName;
                        }
                        model.SubscriptionId = UserRecord.SubscriptionId;
                        if (model.Id == 0)
                        {
                            var GetLastEmployee = dbContext.Employees.Where(s => s.SubscriptionId == UserRecord.SubscriptionId &&
                            !s.IsDeleted &&
                            s.OrganizationId == model.OrganizationId).AsQueryable();
                            if (GetLastEmployee.Count() == 0)
                            {
                                model.Code = ParseInt(organizations.EmployeeCodeStart);
                                model.EmployeeCode = organizations.Code + "-" + organizations.EmployeeCodeStart;
                            }
                            else
                            {
                                model.Code = ParseInt(GetLastEmployee.Max(s => s.Code).GetValueOrDefault() + 1);
                                int len = organizations.EmployeeCodeStart.Length;

                                model.EmployeeCode = organizations.Code + "-" + string.Format("{0:D" + len + "}", model.Code);
                            }
                            UserAccounts user = new UserAccounts();
                            user.Username = model.EmployeeCode;
                            user.SubscriptionId = UserRecord.SubscriptionId;
                            user.OrganizationId = model.OrganizationId;
                            user.Password = Encrypt("123");
                            user.Email = model.Email == null ? "" : model.Email;
                            user.Status = EnumEnableDisable.Enable;
                            user.Phone = model.Phone.ToString();
                            user.RoleId = dbContext.Roles.FirstOrDefault(x => x.Name.Equals(EnumOrganizationRoles.Employee) && x.OrganizationId == organizations.Id && !x.IsDeleted).Id;
                            user.CreatedDateTime = GetDateTime(UserRecord.SubscriptionTimeZone);
                            user.UtcCreatedDateTime = GetUtcDateTime();
                            user.CreatedBy = UserRecord.UserId;
                            user.Name = model.Firstname + " " + model.Lastname;
                            if (FilePath != "")
                            {
                                model.ProfileImage = FilePath;
                                user.ProfileImage = FilePath;
                            }
                            dbContext.UserAccounts.Add(user);
                            dbContext.SaveChanges();
                            model.UserId = user.Id;
                            model.CreatedBy = UserRecord.UserId;
                            model.CreatedDateTime = GetDateTime(UserRecord.SubscriptionTimeZone);
                            model.UtcCreatedDateTime = GetUtcDateTime();
                            dbContext.Employees.Add(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Saved Successfully";
                        }
                        else
                        {
                            var employee = dbContext.Employees.FirstOrDefault(x => x.Id == model.Id);
                            model.Code = employee.Code;
                            model.EmployeeCode = employee.EmployeeCode;
                            var user = employee.User;
                            if (FilePath != "")
                            {
                                model.ProfileImage = FilePath;
                                user.ProfileImage = FilePath;
                            }
                            dbContext.Entry(employee).State = EntityState.Detached;
                            user.Email = model.Email == null ? "" : model.Email;
                            dbContext.UserAccounts.Update(user);
                            dbContext.SaveChanges();
                            model.UserId = user.Id;
                            model.UpdatedBy = UserRecord.UserId;
                            model.UpdatedDateTime = GetDateTime(UserRecord.SubscriptionTimeZone);
                            model.UtcUpdatedDateTime = GetUtcDateTime();
                            dbContext.Employees.Update(model);
                            ajaxResponse.Success = true;
                            ajaxResponse.Message = "Record Updated Successfully";
                        }
                        dbContext.SaveChanges();
                        ajaxResponse.TargetURL = ViewBag.WebsiteURL + "employees";
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
                        var RecordToDelete = dbContext.Employees.FirstOrDefault(o => o.Id == Id && o.SubscriptionId == UserRecord.SubscriptionId && o.OrganizationId == UserRecord.OrganizationId);
                        if (RecordToDelete != null)
                        {
                            RecordToDelete.IsDeleted = true;
                            RecordToDelete.DeletedBy = UserRecord.UserId;
                            RecordToDelete.DeletedDate = GetDateTime(UserRecord.SubscriptionTimeZone);
                            dbContext.Employees.Update(RecordToDelete);
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
