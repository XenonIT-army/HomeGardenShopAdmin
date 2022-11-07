using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Entities
{
    [Table("AboutUs")]
    public class AboutUsDB
    {
        public int Id { get; set; }
        public string NameCompany { get; set; }

        public string Description { get; set; }

        public string DescriptionEN { get; set; }

        public string DescriptionUA { get; set; }
    }
}
