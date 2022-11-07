using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Entities
{
    [Table("Products")]
    public partial class ProductDB
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        [Required]
        [StringLength(40)]
        public string NameUA { get; set; }
        [Required]
        [StringLength(40)]
        public string NameEN { get; set; }
        public double Count { get; set; }

        public double Price { get; set; }

        public int CategoryId { get; set; }
        public double DiscountPrice { get; set; }

        public string Description { get; set; }
        public string DescriptionEN { get; set; }

        public string DescriptionUA { get; set; }

        public byte[] Image { get; set; }
    }

}
