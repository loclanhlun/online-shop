using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities
{
    public class Cart
    {
        //id
        [Key]
        public int Id   { get; set; }
        //quantity
        public int Quantity { get; set; }
        //product id
        public int productId { get; set; }
        public int userId { get; set; }
        [ForeignKey("FK_Cart_product")]
        public Product product { set; get; }
        [ForeignKey("FK_Cart_user")]
        public User user { set; get; }
    }
}
