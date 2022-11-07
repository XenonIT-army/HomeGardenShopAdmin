using AutoMapper;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.UnitOfWork;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;

namespace HomeGardenShopAdmin.Moduls
{
    public class AboutUsService : IService<AboutUs>
    {
        private ProductUnitOfWork uow;
        IMapper mapper;


        public AboutUsService(ProductUnitOfWork uow)
        {
            this.uow = uow;
            var config = new MapperConfiguration(cfg =>
            cfg
            .CreateMap<AboutUsDB, AboutUs>().ReverseMap());
            mapper = config.CreateMapper();
        }

        public Task<AboutUs> Create(AboutUs dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<AboutUsDB>(dto);
                return mapper.Map<AboutUs>(null);
            });
        }

        public Task<bool> Delete(AboutUs dto)
        {
            return Task.Run(() =>
            {
                var entity = uow.AboutUsRepository.Get(dto.Id);
                uow.AboutUsRepository.Delete(entity);
                return true;
            });
        }

        public Task<AboutUs> Get(int id)
        {
            return Task.Run(() =>
            {
                var entity = uow.AboutUsRepository.Get(id);
                return mapper.Map<AboutUs>(entity);
            });
        }

        public Task<IEnumerable<AboutUs>> GetAll()
        {
            try
            {
                return Task.Run(() =>
                {
                    var res = uow.AboutUsRepository
                   .GetAll()
                   .ToList()
                   .Select(entity => mapper.Map<AboutUs>(entity));
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
                uow.AboutUsRepository.Save();
                uow.Save();
                return true;
            });
        }
        public Task<bool> AddOrUpdate(AboutUs dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<AboutUsDB>(dto);
                uow.AboutUsRepository.AddOrUpdate(entity);
                return true;
            });
        }

        public Task<bool> Update(AboutUs dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<AboutUsDB>(dto);
                uow.AboutUsRepository.Update(entity);
                return true;
            });
        }

        public Task<int> GetLast()
        {
            return Task.Run(() =>
            {
                var entity = uow.AboutUsRepository.GetLast(p => p.Id);
                return mapper.Map<int>(entity);
            });
        }
    }
}
