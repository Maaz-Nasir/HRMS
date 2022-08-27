using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Subscriptions
    {
        public Subscriptions()
        {
            Cities = new HashSet<Cities>();
            Departments = new HashSet<Departments>();
            Designations = new HashSet<Designations>();
            Employees = new HashSet<Employees>();
            ExpenseTypes = new HashSet<ExpenseTypes>();
            Expenses = new HashSet<Expenses>();
            Organizations = new HashSet<Organizations>();
            Regions = new HashSet<Regions>();
            Roles = new HashSet<Roles>();
            SubscriptionModules = new HashSet<SubscriptionModules>();
            UserAccounts = new HashSet<UserAccounts>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string TimeZoneId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NoOfMonth { get; set; }
        public string ContactName { get; set; }
        public decimal? ContactNo { get; set; }
        public string ContactEmail { get; set; }
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
        public string SubscriptionUrl { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<Departments> Departments { get; set; }
        public virtual ICollection<Designations> Designations { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<ExpenseTypes> ExpenseTypes { get; set; }
        public virtual ICollection<Expenses> Expenses { get; set; }
        public virtual ICollection<Organizations> Organizations { get; set; }
        public virtual ICollection<Regions> Regions { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<SubscriptionModules> SubscriptionModules { get; set; }
        public virtual ICollection<UserAccounts> UserAccounts { get; set; }
    }
}
