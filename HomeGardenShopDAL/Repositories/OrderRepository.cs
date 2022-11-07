using HomeGardenShopDAL.Abstructions;
using HomeGardenShopDAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Repositories
{
    public class OrderRepository : BaseRepository<OrderDB>
    {
        public OrderRepository(DbContext db) : base(db)
        {
        }
    }
}
