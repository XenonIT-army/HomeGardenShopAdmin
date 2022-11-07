namespace HomeGardenShopAdmin.Models
{
    public class News
    {
		public int Id { get; set; }
		public string Name { get; set; }

        public string? NameUA { get; set; }
        public string? NameEN { get; set; }

        public string Description { get; set; }

        public string? DescriptionEN { get; set; }

        public string? DescriptionUA { get; set; }

        public byte[]? Image { get; set; }

		public DateTime DateTime { get; set; }
	}
}
