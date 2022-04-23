using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities
{
    public class Order
    {
        //Id
        [Key]
        public int Id { get; set; }
        //customer id
        public int UserId { get; set; }
        //order date
        public DateTime OrderDate { get; set; }
        //Status
        public string OrderStatus { get; set; }
        public string AddressShipping { get; set; }
        //Note
        public string Note { get; set; }
        //Fk with user
        [ForeignKey("FK_Order_User")]
        public User user { set; get; }
    }
}
