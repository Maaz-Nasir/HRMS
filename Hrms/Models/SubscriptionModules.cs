using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class SubscriptionModules
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public int MenuId { get; set; }
        public string Type { get; set; }

        public virtual Menus Menu { get; set; }
        public virtual Subscriptions Subscription { get; set; }
    }
}
