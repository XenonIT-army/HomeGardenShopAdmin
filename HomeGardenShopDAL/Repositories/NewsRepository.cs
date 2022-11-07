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
    public class NewsRepository : BaseRepository<NewsDB>
    {
        public NewsRepository(DbContext db) : base(db)
        {
        }
    }
}
