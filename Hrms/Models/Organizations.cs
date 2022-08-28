using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Organizations
    {
        public Organizations()
        {
            Cities = new HashSet<Cities>();
            Departments = new HashSet<Departments>();
            Designations = new HashSet<Designations>();
            Employees = new HashSet<Employees>();
            ExpenseTypes = new HashSet<ExpenseTypes>();
            Expenses = new HashSet<Expenses>();
            Regions = new HashSet<Regions>();
            Roles = new HashSet<Roles>();
            UserAccounts = new HashSet<UserAccounts>();
        }

        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public string Code { get; set; }
        public string EmployeeCodeStart { get; set; }
        public string Name { get; set; }
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
        public string Logo { get; set; }

        public virtual Subscriptions Subscription { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<Departments> Departments { get; set; }
        public virtual ICollection<Designations> Designations { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<ExpenseTypes> ExpenseTypes { get; set; }
        public virtual ICollection<Expenses> Expenses { get; set; }
        public virtual ICollection<Regions> Regions { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<UserAccounts> UserAccounts { get; set; }
    }
}
