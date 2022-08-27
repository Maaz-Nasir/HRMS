using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Menus
    {
        public Menus()
        {
            InverseParent = new HashSet<Menus>();
            MenuPermissions = new HashSet<MenuPermissions>();
            RolePermissions = new HashSet<RolePermissions>();
            SubscriptionModules = new HashSet<SubscriptionModules>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string AccessUrl { get; set; }
        public int? SequenceOrder { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UtcCreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual Menus Parent { get; set; }
        public virtual ICollection<Menus> InverseParent { get; set; }
        public virtual ICollection<MenuPermissions> MenuPermissions { get; set; }
        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
        public virtual ICollection<SubscriptionModules> SubscriptionModules { get; set; }
    }
}
