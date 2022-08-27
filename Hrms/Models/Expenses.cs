using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Expenses
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int OrganizationId { get; set; }
        public int? ExpenseTypeId { get; set; }
        public string ExpenseFrom { get; set; }
        public string ExpenseTo { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseMonth { get; set; }
        public string ExpenseYear { get; set; }
        public decimal? Amount { get; set; }
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

        public virtual ExpenseTypes ExpenseType { get; set; }
        public virtual Organizations Organization { get; set; }
        public virtual Subscriptions Subscription { get; set; }
    }
}
