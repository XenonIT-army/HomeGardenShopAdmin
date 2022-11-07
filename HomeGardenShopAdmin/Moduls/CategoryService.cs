using AutoMapper;
using HomeGardenShopDAL.Entities;
using HomeGardenShopDAL.UnitOfWork;
using HomeGardenShopAdmin.Interface;
using HomeGardenShopAdmin.Models;

namespace HomeGardenShopAdmin.Moduls
{
    public class CategoryService : IService<Category>
    {
        private ProductUnitOfWork uow;
        IMapper mapper;


        public CategoryService(ProductUnitOfWork uow)
        {
            this.uow = uow;
            var config = new MapperConfiguration(cfg =>
            cfg
            .CreateMap<CategoryDB, Category>().ReverseMap());
            //.ForMember(x => x.ManufacturerName,
            //opt => opt.MapFrom(x => x.Manufacturer.ManufacturerName ?? null))
            //.ForMember(x => x.CategoryName,
            //opt => opt.MapFrom(x => x.Category.CategoryName ?? null)).ReverseMap());
            mapper = config.CreateMapper();
        }

        public Task<Category> Create(Category dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<CategoryDB>(dto);
                //var newEntity = uow.ProductRepository.Create(entity);
                return mapper.Map<Category>(null);
            });
        }

        public Task<bool> Delete(Category dto)
        {
            return Task.Run(() =>
            {
                var entity = uow.CategoryRepository.Get(dto.Id);
                uow.CategoryRepository.Delete(entity);
                return true;
            });
        }

        public Task<Category> Get(int id)
        {
            return Task.Run(() =>
            {
                var entity = uow.CategoryRepository.Get(id);
                return mapper.Map<Category>(entity);
            });
        }

        public Task<IEnumerable<Category>> GetAll()
        {
            try
            {
                return Task.Run(() =>
                {
                    var res = uow.CategoryRepository
                   .GetAll()
                   .ToList()
                   .Select(entity => mapper.Map<Category>(entity));
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
                uow.CategoryRepository.Save();
                //uow.ManufacturerRepository.Save();
                //uow.CategoryRepository.Save();
                uow.Save();
                return true;
            });
        }
        public Task<bool> AddOrUpdate(Category dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<CategoryDB>(dto);
                uow.CategoryRepository.AddOrUpdate(entity);
                return true;
            });
        }

        public Task<bool> Update(Category dto)
        {
            return Task.Run(() =>
            {
                var entity = mapper.Map<CategoryDB>(dto);
                uow.CategoryRepository.Update(entity);
                return true;
            });
        }

        public Task<int> GetLast()
        {
            return Task.Run(() =>
            {
                var entity = uow.CategoryRepository.GetLast(p=> p.Id);
                return mapper.Map<int>(entity);
            });
        }
    }
}
