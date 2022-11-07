using AutoMapper;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.UnitOfWork;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;

namespace HomeGardenShopAdmin.Moduls
{
    public class ProductOrderService : IService<ProductOrder>
    {
        private ProductUnitOfWork uow;
        IMapper mapper;


        public ProductOrderService(ProductUnitOfWork uow)
        {
            this.uow = uow;
            var config = new MapperConfiguration(cfg =>
            cfg
            .CreateMap<ProductOrderDB, ProductOrder>().ReverseMap());
            mapper = config.CreateMapper();
        }

        public Task<ProductOrder> Create(ProductOrder dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<ProductOrderDB>(dto);
                return mapper.Map<ProductOrder>(null);
            });
        }

        public Task<bool> Delete(ProductOrder dto)
        {
            return Task.Run(() =>
            {
                var entity = uow.ProductOrderRepository.Get(dto.Id);
                uow.ProductOrderRepository.Delete(entity);
                return true;
            });
        }

        public Task<ProductOrder> Get(int id)
        {
            return Task.Run(() =>
            {
                var entity = uow.ProductOrderRepository.Get(id);
                return mapper.Map<ProductOrder>(entity);
            });
        }

        public Task<IEnumerable<ProductOrder>> GetAll()
        {
            try
            {
                return Task.Run(() =>
                {
                    var res = uow.ProductOrderRepository
                   .GetAll()
                   .ToList()
                   .Select(entity => mapper.Map<ProductOrder>(entity));
                    return res;
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<bool> Save()
        {
            return Task.Run(() =>
            {
                uow.ProductOrderRepository.Save();
                uow.Save();
                return true;
            });
        }
        public Task<bool> AddOrUpdate(ProductOrder dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<ProductOrderDB>(dto);
                uow.ProductOrderRepository.AddOrUpdate(entity);
                return true;
            });
        }

        public Task<bool> Update(ProductOrder dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<ProductOrderDB>(dto);
                uow.ProductOrderRepository.Update(entity);
                return true;
            });
        }

        public Task<int> GetLast()
        {
            return Task.Run(() =>
            {
                var entity = uow.ProductOrderRepository.GetLast(p => p.Id);
                return mapper.Map<int>(entity);
            });
        }
    }
}
