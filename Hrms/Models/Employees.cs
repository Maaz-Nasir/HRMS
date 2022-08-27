using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Employees
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int OrganizationId { get; set; }
        public int DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int UserId { get; set; }
        public string EmployeeCode { get; set; }
        public int? Code { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public decimal Phone { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UtcCreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? UtcUpdatedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string ProfileImage { get; set; }

        public virtual Cities City { get; set; }
        public virtual Countries Country { get; set; }
        public virtual Departments Department { get; set; }
        public virtual Designations Designation { get; set; }
        public virtual Organizations Organization { get; set; }
        public virtual Subscriptions Subscription { get; set; }
        public virtual UserAccounts User { get; set; }
    }
}
