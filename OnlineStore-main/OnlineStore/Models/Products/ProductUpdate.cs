using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models.Products
{
    public class ProductUpdate
    {
        public string Name { get; set; }
        public string Image { get; set; }
        [Column(TypeName = "Money")]
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int BrandId { get; set; }
        public int AttributesId { get; set; }
    }
}
