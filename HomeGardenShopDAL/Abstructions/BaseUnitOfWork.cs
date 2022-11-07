using HomeGardenShopDAL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Abstructions
{
    public abstract class BaseUnitOfWork : IUnitOfWork, IDisposable
    {
        protected DbContext db;

        public BaseUnitOfWork(DbContext db)
        {
            this.db = db;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
