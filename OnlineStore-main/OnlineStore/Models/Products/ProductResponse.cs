namespace OnlineStore.Models.Products
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
    }
}
