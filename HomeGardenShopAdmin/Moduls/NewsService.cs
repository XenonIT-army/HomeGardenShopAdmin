using AutoMapper;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.UnitOfWork;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;

namespace HomeGardenShopAdmin.Moduls
{
    public class NewsService : IService<News>
    {
        private ProductUnitOfWork uow;
        IMapper mapper;


        public NewsService(ProductUnitOfWork uow)
        {
            this.uow = uow;
            var config = new MapperConfiguration(cfg =>
            cfg
            .CreateMap<NewsDB, News>().ReverseMap());
            mapper = config.CreateMapper();
        }

        public Task<News> Create(News dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<NewsDB>(dto);
                return mapper.Map<News>(null);
            });
        }

        public Task<bool> Delete(News dto)
        {
            return Task.Run(() =>
            {
                var entity = uow.NewsRepository.Get(dto.Id);
                uow.NewsRepository.Delete(entity);
                return true;
            });
        }

        public Task<News> Get(int id)
        {
            return Task.Run(() =>
            {
                var entity = uow.NewsRepository.Get(id);
                return mapper.Map<News>(entity);
            });
        }

        public Task<IEnumerable<News>> GetAll()
        {
            try
            {
                return Task.Run(() =>
                {
                    var res = uow.NewsRepository
                   .GetAll()
                   .ToList()
                   .Select(entity => mapper.Map<News>(entity));
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
                uow.NewsRepository.Save();
                //uow.ManufacturerRepository.Save();
                //uow.CategoryRepository.Save();
                uow.Save();
                return true;
            });
        }
        public Task<bool> AddOrUpdate(News dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<NewsDB>(dto);
                uow.NewsRepository.AddOrUpdate(entity);
                return true;
            });
        }

        public Task<bool> Update(News dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<NewsDB>(dto);
                uow.NewsRepository.Update(entity);
                return true;
            });
        }

        public Task<int> GetLast()
        {
            return Task.Run(() =>
            {
                var entity = uow.NewsRepository.GetLast(p => p.Id);
                return mapper.Map<int>(entity);
            });
        }
    }
}
