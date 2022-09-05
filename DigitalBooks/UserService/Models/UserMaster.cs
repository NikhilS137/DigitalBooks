using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models
{
    [Table("UserMaster")]
    public partial class UserMaster
    {
        public UserMaster()
        {
            BookMasters = new HashSet<BookMaster>();
        }

        [Key]
        public int UserId { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string UserName { get; set; } = null!;
        [StringLength(300)]
        [Unicode(false)]
        public string EmailId { get; set; } = null!;
        [StringLength(500)]
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public bool Active { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;

        [ForeignKey("RoleId")]
        [InverseProperty("UserMasters")]
        public virtual RoleMaster? Role { get; set; } = null!;
        [InverseProperty("User")]
        public virtual ICollection<BookMaster> BookMasters { get; set; }
    }
}
