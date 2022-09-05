using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models
{
    [Table("BookMaster")]
    public partial class BookMaster
    {
        public BookMaster()
        {
            Purchases = new HashSet<Purchase>();
        }

        [Key]
        public int BookId { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string BookName { get; set; } = null!;
        public int CategoryId { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string Publisher { get; set; } = null!;
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PublishedDate { get; set; }
        [Column(TypeName = "ntext")]
        public string BookContent { get; set; } = null!;
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? Createdby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public int? Modifiedby { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("BookMasters")]
        public virtual CategoryMaster? Category { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("BookMasters")]
        public virtual UserMaster? User { get; set; } = null!;
        [InverseProperty("Book")]
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
