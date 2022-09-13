using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Models
{
    [Table("Purchase")]
    public partial class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }
        [StringLength(300)]
        [Unicode(false)]
        public string EmailId { get; set; } = null!;
        public int BookId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PurchaseDate { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string PaymentMode { get; set; } = null!;
        [StringLength(10)]
        public string? IsRefunded { get; set; }

        [ForeignKey("BookId")]
        [InverseProperty("Purchases")]
        public virtual BookMaster? Book { get; set; } = null!;
    }
}
