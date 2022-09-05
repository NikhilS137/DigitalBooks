using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models
{
    [Table("CategoryMaster")]
    public partial class CategoryMaster
    {
        public CategoryMaster()
        {
            BookMasters = new HashSet<BookMaster>();
        }

        [Key]
        public int CategoryId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string CategoryName { get; set; } = null!;

        [InverseProperty("Category")]
        public virtual ICollection<BookMaster> BookMasters { get; set; }
    }
}
