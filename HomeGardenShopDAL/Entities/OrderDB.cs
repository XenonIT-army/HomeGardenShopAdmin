using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Entities
{
    [Table("Order")]
    public partial class OrderDB
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        public int StatusId { get; set; }

        public double Sum { get; set; }

        public DateTime DateTime { get; set; }

    }
  
}
