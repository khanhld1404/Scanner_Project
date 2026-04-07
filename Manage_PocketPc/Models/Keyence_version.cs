using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_PocketPc.Models
{
    [Table("Keyence_version")]
    public partial class Keyence_version
    {
        [Key]
        public int Version { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_time { get; set; }
        [StringLength(300)]
        public string? Comment { get; set; }
    }
}
