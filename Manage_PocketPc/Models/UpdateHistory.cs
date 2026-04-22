using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_PocketPc.Models
{
    [Table("UpdateHistory")]
    public partial class UpdateHistory
    {
        [Key]
        [Required(ErrorMessage = "Mời bạn nhập phiên bản phần mềm")]
        public int Version { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Mời bạn nhập mã nhân viên")]
        public string? Person { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Time { get; set; }
        [StringLength(500)]
        [Required(ErrorMessage = "Mời bạn nhập nội dung cập nhật")]
        public string? Contents { get; set; }
    }
}
