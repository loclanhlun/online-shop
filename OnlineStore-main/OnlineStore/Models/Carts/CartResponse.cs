namespace OnlineStore.Models.Carts
{
    public class CartResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public string Content { get; set; }
        public decimal ? Price { get; set; }
        public int Quantity { get; set; }
        public decimal? Total { get; set; }

    }
}
