using System;

namespace OnlineStore.Models.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        //User Name
        public string CustomerName { get; set; }
        //Address
        public string AddressShipping { get; set; }
        //order date
        public DateTime OrderDate { get; set; }
        
        //Status
        public string OrderStatus { get; set; }
        //Note
        public string Note { get; set; }
    }
}
