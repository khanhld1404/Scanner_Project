using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Manage_PocketPc.Models
{
    [Table("LoginUser")]
    public partial class LoginUser
    {
        [Key]
        [StringLength(50)]
        public string user_code { get; set; } = null!;
        [StringLength(200)]
        [Required(ErrorMessage = "Mời bạn nhập mật khẩu")]
        public string? password { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Mời bạn nhập an bộ phận")]
        public string? department { get; set; }

        [NotMapped]
        public string SimplePassword { get; set; }
    }
}
