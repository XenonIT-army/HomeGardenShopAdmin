using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Entities
{
    [Table("ProductOrder")]
    public partial class ProductOrderDB
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public double Count { get; set; }
        public double Price { get; set; }
    }
}
