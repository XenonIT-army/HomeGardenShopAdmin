using HomeGardenShopDAL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Abstructions
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        protected DbContext db;

        public BaseRepository(DbContext db)
        {
            this.db = db;
        }

        public TEntity Create(TEntity entity)
        {
            return db.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public TEntity Get(int id)
        {
            return db.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return db.Set<TEntity>();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            db.Entry(entity).State = EntityState.Modified;
        }
        public void AddOrUpdate(TEntity entity)
        {
            db.Set<TEntity>().AddOrUpdate(entity);

        }

        public int GetLast(Func<TEntity,int> key)
        {
            return db.Set<TEntity>().Max(key);
        }
    }
}
