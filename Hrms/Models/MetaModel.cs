using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hrms.Models
{
    [ModelMetadataType(typeof(EmployeesModelMetaData))]
    public partial class Employees
    {
    }
    public class EmployeesModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int DesignationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Please correct your email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(DesignationsModelMetaData))]
    public partial class Designations
    {
    }
    public class DesignationsModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(DepartmentsModelMetaData))]
    public partial class Departments
    {
    }
    public class DepartmentsModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(CitiesModelMetaData))]
    public partial class Cities
    {
    }
    public class CitiesModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int RegionId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(RegionsModelMetaData))]
    public partial class Regions
    {
    }
    public class RegionsModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(ExpenseTypesModelMetaData))]
    public partial class ExpenseTypes
    {
    }
    public class ExpenseTypesModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(ExpensesModelMetaData))]
    public partial class Expenses
    {
    }
    public class ExpensesModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int ExpenseTypeId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ExpenseFrom { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ExpenseTo { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ExpenseDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ExpenseYear { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ExpenseMonth { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(UserAccountsModelMetaData))]
    public partial class UserAccounts
    {
    }
    public class UserAccountsModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter proper email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(RolesModelMetaData))]
    public partial class Roles
    {
    }
    public class RolesModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int OrganizationId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
    }
    [ModelMetadataType(typeof(OrganizationsModelMetaData))]
    public partial class Organizations
    {
    }
    public class OrganizationsModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
    }
    [ModelMetadataType(typeof(SubscriptionsModelMetaData))]
    public partial class Subscriptions
    {
        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [NotMapped]
        public int UserId { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Required")]
        public string EmailAddress { get; set; }
    }
    public class SubscriptionsModelMetaData
    {
        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string TimeZoneId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required")]
        public string StartDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string EndDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public decimal NoOfMonth { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ContactName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ContactEmail { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ContactNo { get; set; }
    }
    public class UserSessionData
    {
        public int UserId { get; set; }
        public int SubscriptionId { get; set; }
        public int? SubscriptionCountryId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string SubscriptionCountryName { get; set; }
        public string SubscriptionCode { get; set; }
        public string SubscriptionTimeZone { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string LoginCode { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileImageWithPath { get; set; }
        public string SubDomain { get; set; }
        public int? EmployeeId { get; set; }
        public int? OrganizationId { get; set; }
    }
}
