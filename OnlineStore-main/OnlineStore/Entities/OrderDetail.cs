using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnlineStore.Entities
{
    public class OrderDetail
    {
        //id
        [Key]
        public int Id { get; set; }
     
        //order id
        public int OrderId { get; set; }
        public string productName { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Total { get; set; }
        public string PaymentStatus { get; set; }

        [ForeignKey("FK_OrderDetail_order")]
        public Order order { set; get; }
        public string AddressShipping { get; internal set; }
        public string Status { get; internal set; }
        public DateTime OrderDate { get; internal set; }
        public string Note { get; internal set; }
    }
}
