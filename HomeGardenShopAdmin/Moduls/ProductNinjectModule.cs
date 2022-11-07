using HomeGardenShopDAL.Context;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.Interface;
using HomeGardenShopDAL.Repositories;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;
using Ninject.Modules;
using System.Data.Entity;

namespace HomeGardenShopAdmin.Moduls
{
    public class ProductNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<ProductDB>>().To<ProductRepository>();
            Bind<IRepository<CategoryDB>>().To<CategoryRepository>();
            Bind<IRepository<OrderDB>>().To<OrderRepository>();
            Bind<IRepository<ProductOrderDB>>().To<ProductOrderRepository>();
            Bind<IRepository<NewsDB>>().To<NewsRepository>();
            Bind<IRepository<UserDB>>().To<UserRepository>();
            Bind<IRepository<AboutUsDB>>().To<AboutUsRepository>();

            Bind<IService<Product>>().To<ProductService>();
            Bind<IService<Category>>().To<CategoryService>();
            Bind<IService<Order>>().To<OrderService>();
            Bind<IService<ProductOrder>>().To<ProductOrderService>();
            Bind<IService<News>>().To<NewsService>();
            Bind<IService<User>>().To<UserService>();
            Bind<IService<AboutUs>>().To<AboutUsService>();

            Bind<DbContext>().To<ShopContext>();
        }
    }
}
