using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_PocketPc.Models
{
    [Table("IK")]
    public partial class IK
    {
        public long? an { get; set; }
        [Key]
        [StringLength(50)]
        public string product { get; set; } = null!;
    }
}
