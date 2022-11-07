using System;
using System.ComponentModel.DataAnnotations;

namespace HomeGardenShopAdmin.Models
{
	public class Product
	{

		public int Id { get; set; }
		public string Name { get; set; }
        public string? NameUA { get; set; }
        public string? NameEN { get; set; }

        public double Count { get; set; }

		public double Price { get; set; }

		public int CategoryId { get; set; }

		public double DiscountPrice { get; set; }

		public string Description { get; set; }
        public string? DescriptionEN { get; set; }

        public string? DescriptionUA { get; set; }

        public byte[] Image { get; set; }
	}
}

