namespace HomeGardenShopAdmin.Models
{
    public class ProductOrder
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int OrderId { get; set; }
        public double Count { get; set; }
        public double Price { get; set; }
    }
}
