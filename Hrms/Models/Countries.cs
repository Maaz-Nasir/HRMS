using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Countries
    {
        public Countries()
        {
            Cities = new HashSet<Cities>();
            Employees = new HashSet<Employees>();
            Regions = new HashSet<Regions>();
            Subscriptions = new HashSet<Subscriptions>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<Regions> Regions { get; set; }
        public virtual ICollection<Subscriptions> Subscriptions { get; set; }
    }
}
