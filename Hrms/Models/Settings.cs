using System;
using System.Collections.Generic;

namespace Hrms.Models
{
    public partial class Settings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}
