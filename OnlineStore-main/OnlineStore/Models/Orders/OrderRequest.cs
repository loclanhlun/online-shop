using System;

namespace OnlineStore.Models.Orders
{
    public class OrderRequest
    {
        //customer id
        //public int UserId { get; set; }
        //Note
        public string Note { get; set; }
        public string AddressShipping { get; set; }
    }
}
