using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_PocketPc.Models
{
    public partial class ScanDatum
    {
        [StringLength(50)]
        public string User_code { get; set; } = null!;
        [StringLength(50)]
        public string Department { get; set; } = null!;
        [StringLength(100)]
        public string Master_code { get; set; } = null!;
        public int OK_count { get; set; }
        public int NG_count { get; set; }
        [StringLength(50)]
        public string? DeviceNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
