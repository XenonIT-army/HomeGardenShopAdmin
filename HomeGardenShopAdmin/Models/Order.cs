namespace HomeGardenShopAdmin.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int StatusId { get; set; }

        public double Sum { get; set; }

        public DateTime DateTime { get; set; }
    }

    public enum OrderStatus
    {
        Error = 1,
        Make = 2,
        InProcess = 3,
        Formed = 4,
        Complete = 5,
        Canceled = 6
    }
}
