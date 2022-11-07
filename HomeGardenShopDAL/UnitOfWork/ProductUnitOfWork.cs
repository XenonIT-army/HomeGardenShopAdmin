using HomeGardenShopDAL.Abstructions;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeGardenShopDAL.UnitOfWork
{
    public class ProductUnitOfWork : BaseUnitOfWork
    {
        public IRepository<ProductDB> ProductRepository { get; }
        public IRepository<CategoryDB> CategoryRepository { get; }
        public IRepository<OrderDB> OrderRepository { get; }
        public IRepository<ProductOrderDB> ProductOrderRepository { get; }

        public IRepository<NewsDB> NewsRepository { get; }

        public IRepository<UserDB> UserRepository { get; }

        public IRepository<AboutUsDB> AboutUsRepository { get; }

        public ProductUnitOfWork(DbContext db,
                                 IRepository<ProductDB> productRepo,
                                 IRepository<CategoryDB> categoryRepo,
                                 IRepository<OrderDB> orderRepo,
                                 IRepository<ProductOrderDB> productOrderRepo,
                                 IRepository<NewsDB> newsRepository,
                                 IRepository<UserDB> userRepository,
                                 IRepository<AboutUsDB> aboutUsRepository) : base(db)
        {

            this.ProductRepository = productRepo;
            this.CategoryRepository = categoryRepo;
            this.OrderRepository = orderRepo;
            this.ProductOrderRepository = productOrderRepo;
            this.NewsRepository = newsRepository;
            this.UserRepository = userRepository;
            this.AboutUsRepository = aboutUsRepository;
        }

    }
}
