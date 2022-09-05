using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Models
{
    [Table("RoleMaster")]
    public partial class RoleMaster
    {
        public RoleMaster()
        {
            UserMasters = new HashSet<UserMaster>();
        }

        [Key]
        public int RoleId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string RoleName { get; set; } = null!;

        [InverseProperty("Role")]
        public virtual ICollection<UserMaster> UserMasters { get; set; }
    }
}
