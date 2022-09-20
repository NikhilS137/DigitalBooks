using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
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

        public Boolean callPaymentAuzreFunPost()
        {
            bool retVal = false;
            //string URL = "http://localhost:7009/api/PaymentFunction";
            string URL = "https://paymentfunctionapp20220919120647.azurewebsites.net/api/PaymentFunction";
            string urlParameters = "?code=A671q9htefJe4eySIAZ4T6zn3b7A6Yp3iQFTlGyZGDe1AzFuGgXK7g==";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            var myJson = "{ \"emailID\" : \"" + EmailId + "\"," +
                            "\"bookID\" : \"" + BookId + "\"," +
                            "\"paymentMode\" : \"" + PaymentMode + "\"}";

            // List data response.
            HttpResponseMessage response = client.PostAsync(urlParameters, new StringContent(myJson, Encoding.UTF8, "application/json")).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                //response.Content.ReadAsStringAsync().Result;
                retVal = true;
            }
            else
            {
                //res = response.Content.ReadAsStringAsync().Result;
            }
            client.Dispose();
            return retVal;
        }
    }
}
