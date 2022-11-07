using AutoMapper;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.UnitOfWork;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;

namespace HomeGardenShopAdmin.Moduls
{
    public class UserService : IService<User>
    {
        private ProductUnitOfWork uow;
        IMapper mapper;


        public UserService(ProductUnitOfWork uow)
        {
            this.uow = uow;
            var config = new MapperConfiguration(cfg =>
            cfg
            .CreateMap<UserDB, User>().ReverseMap());
            mapper = config.CreateMapper();
        }

        public Task<User> Create(User dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<UserDB>(dto);
                return mapper.Map<User>(null);
            });
        }

        public Task<bool> Delete(User dto)
        {
            return Task.Run(() =>
            {
                var entity = uow.UserRepository.Get(dto.Id);
                uow.UserRepository.Delete(entity);
                return true;
            });
        }

        public Task<User> Get(int id)
        {
            return Task.Run(() =>
            {
                var entity = uow.UserRepository.Get(id);
                return mapper.Map<User>(entity);
            });
        }

        public Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return Task.Run(() =>
                {
                    var res = uow.UserRepository
                   .GetAll()
                   .ToList()
                   .Select(entity => mapper.Map<User>(entity));
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
                uow.UserRepository.Save();
                uow.Save();
                return true;
            });
        }
        public Task<bool> AddOrUpdate(User dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<UserDB>(dto);
                uow.UserRepository.AddOrUpdate(entity);
                return true;
            });
        }

        public Task<bool> Update(User dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<UserDB>(dto);
                uow.UserRepository.Update(entity);
                return true;
            });
        }

        public Task<int> GetLast()
        {
            return Task.Run(() =>
            {
                var entity = uow.UserRepository.GetLast(p => p.Id);
                return mapper.Map<int>(entity);
            });
        }
    }
}
