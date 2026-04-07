using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_PocketPc.Models
{
    [Table("RFC")]
    public partial class RFC
    {
        [Required(ErrorMessage = "Mời bạn nhập an của RFC")]
        public long? an { get; set; }
        [Key]
        [StringLength(50)]
        public string product { get; set; } = null!;
    }
}
