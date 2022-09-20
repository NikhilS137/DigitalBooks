using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PaymentFunctionApp
{
    public class PurchaseModel
    {
        public string EmailId { get; set; } = null!;
        public int BookId { get; set; }
        public string PaymentMode { get; set; } = null!;
    }
}