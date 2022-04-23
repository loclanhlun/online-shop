using System;

namespace OnlineStore.Models.Orders
{
    public class OrderDetailResponse
    {
        public string productName { get; set; }
        public string AddressShipping { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public decimal ? Price { get; set; }
        public decimal? Total { get; set; }
        public string PaymentStatus { get; set; }
    }
}
