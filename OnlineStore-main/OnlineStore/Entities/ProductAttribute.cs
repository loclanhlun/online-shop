using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Entities
{
    public class ProductAttribute
    { 
        [Key]
        public int Id { get; set; }
        public string ColorName { get; set; }
        public string Description { get; set; }
    }
}
