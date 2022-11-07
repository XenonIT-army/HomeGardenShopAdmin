using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Entities
{
    [Table("User")]
    public class UserDB
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Name { get; set; }

        public string Mail { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string UserId { get; set; }
    }
}
