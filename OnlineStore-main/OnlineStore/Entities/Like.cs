using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("FK_Like_User")]
        public User user { get; set; }
        [ForeignKey("FK_Like_Product")]
        public Product product { get; set; }
    }
}
