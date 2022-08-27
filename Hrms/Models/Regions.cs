using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Regions
    {
        public Regions()
        {
            Cities = new HashSet<Cities>();
        }

        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int OrganizationId { get; set; }
        public int CountryId { get; set; }
        public string Code { get; set; }
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

        public virtual Countries Country { get; set; }
        public virtual Organizations Organization { get; set; }
        public virtual Subscriptions Subscription { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
    }
}
