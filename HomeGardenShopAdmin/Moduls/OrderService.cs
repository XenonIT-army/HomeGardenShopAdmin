using AutoMapper;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.UnitOfWork;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;

namespace HomeGardenShopAdmin.Moduls
{
    public class OrderService : IService<Order>
    {
        private ProductUnitOfWork uow;
        IMapper mapper;


        public OrderService(ProductUnitOfWork uow)
        {
            this.uow = uow;
            var config = new MapperConfiguration(cfg =>
            cfg
            .CreateMap<OrderDB, Order>().ReverseMap());
            mapper = config.CreateMapper();
        }

        public Task<Order> Create(Order dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<OrderDB>(dto);
                return mapper.Map<Order>(null);
            });
        }

        public Task<bool> Delete(Order dto)
        {
            return Task.Run(() =>
            {
                var entity = uow.OrderRepository.Get(dto.Id);
                uow.OrderRepository.Delete(entity);
                return true;
            });
        }

        public Task<Order> Get(int id)
        {
            return Task.Run(() =>
            {
                var entity = uow.OrderRepository.Get(id);
                return mapper.Map<Order>(entity);
            });
        }

        public Task<IEnumerable<Order>> GetAll()
        {
            try
            {
                return Task.Run(() =>
                {
                    var res = uow.OrderRepository
                   .GetAll()
                   .ToList()
                   .Select(entity => mapper.Map<Order>(entity));
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
                uow.OrderRepository.Save();
                uow.Save();
                return true;
            });
        }
        public Task<bool> AddOrUpdate(Order dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<OrderDB>(dto);
                uow.OrderRepository.AddOrUpdate(entity);
                return true;
            });
        }

        public Task<bool> Update(Order dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<OrderDB>(dto);
                uow.OrderRepository.Update(entity);
                return true;
            });
        }

        public Task<int> GetLast()
        {
            return Task.Run(() =>
            {
                var entity = uow.OrderRepository.GetLast(p => p.Id);
                return mapper.Map<int>(entity);
            });
        }
    }
}
