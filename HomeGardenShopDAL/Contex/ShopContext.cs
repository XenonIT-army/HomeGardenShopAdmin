using HomeGardenShopDAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.Context
{
    
    public partial class ShopContext : DbContext
    {
        //public ShopContext()
        //    : base("name=WebApiDatabase")
        //{
        //}
        static string connectionString1 = "data source=DESKTOP-J90LF3F\\SQLEXPRESS,49100;initial catalog=HomeGardenDB; integrated security=True; MultipleActiveResultSets=True;App=EntityFramework";

        static string connectionString2 = "data source=TESTSERVER,1433;initial catalog=HomeGardenDB; integrated security=True; MultipleActiveResultSets=True;App=EntityFramework";
        public ShopContext()
            : base(connectionString2)
        {
        }
       
        public virtual DbSet<ProductDB> Product { get; set; }

        public virtual DbSet<CategoryDB> Category { get; set; }

        public virtual DbSet<OrderDB> Order { get; set; }

        public virtual DbSet<NewsDB> News { get; set; }

        public virtual DbSet<ProductOrderDB> ProductOrder { get; set; }

        public virtual DbSet<UserDB> User { get; set; }

        public virtual DbSet<AboutUsDB> AboutUs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        //    modelBuilder.Entity<ProductDB>()
        //        .Property(e => e.GoodName)
        //        .IsUnicode(false);

        //    modelBuilder.Entity<Good>()
        //        .Property(e => e.Price)
        //        .HasPrecision(19, 4);

        //    modelBuilder.Entity<Good>()
        //        .Property(e => e.GoodCount)
        //        .HasPrecision(18, 3);
        }
    }
}
