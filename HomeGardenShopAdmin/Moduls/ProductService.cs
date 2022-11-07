using AutoMapper;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.UnitOfWork;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;

namespace HomeGardenShopAdmin.Moduls
{
    public class ProductService : IService<Product>
    {
        private ProductUnitOfWork uow;
        IMapper mapper;


        public ProductService(ProductUnitOfWork uow)
        {
            this.uow = uow;
            var config = new MapperConfiguration(cfg =>
            cfg
            .CreateMap<ProductDB, Product>().ReverseMap());
            //.ForMember(x => x.ManufacturerName,
            //opt => opt.MapFrom(x => x.Manufacturer.ManufacturerName ?? null))
            //.ForMember(x => x.CategoryName,
            //opt => opt.MapFrom(x => x.Category.CategoryName ?? null)).ReverseMap());
            mapper = config.CreateMapper();
        }

        public Task<Product> Create(Product dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<ProductDB>(dto);
                //var newEntity = uow.ProductRepository.Create(entity);
                return mapper.Map<Product>(null);
            });
        }

        public Task<bool> Delete(Product dto)
        {
            return Task.Run(() =>
            {
                var entity = uow.ProductRepository.Get(dto.Id);
                uow.ProductRepository.Delete(entity);
                return true;
            });
        }

        public Task<Product> Get(int id)
        {
            return Task.Run(() =>
            {
                var entity = uow.ProductRepository.Get(id);
                return mapper.Map<Product>(entity);
            });
        }

        public Task<IEnumerable<Product>> GetAll()
        {
            try
            {
                return Task.Run(() =>
                {
                    var res = uow.ProductRepository
                   .GetAll()
                   .ToList()
                   .Select(entity => mapper.Map<Product>(entity));
                    return res;
                });
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public Task<bool> Save()
        {
            return Task.Run(() =>
            {
                uow.ProductRepository.Save();
                //uow.ManufacturerRepository.Save();
                //uow.CategoryRepository.Save();
                uow.Save();
                return true;
            });
        }
        public Task<bool> AddOrUpdate(Product dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<ProductDB>(dto);
                uow.ProductRepository.AddOrUpdate(entity);
                return true;
            });
        }

        public Task<bool> Update(Product dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<ProductDB>(dto);
                uow.ProductRepository.Update(entity);
                return true;
            });
        }

        public Task<int> GetLast()
        {
            return Task.Run(() =>
            {
                var entity = uow.ProductRepository.GetLast(p => p.Id);
                return mapper.Map<int>(entity);
            });
        }
    }
}
