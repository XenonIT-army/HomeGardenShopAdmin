namespace HomeGardenShopAdmin.Interface
{
    public interface IService<TEntityDto>
    {
        Task<IEnumerable<TEntityDto>> GetAll();
        Task<TEntityDto> Get(int id);
        Task<TEntityDto> Create(TEntityDto dto);
        Task<bool> AddOrUpdate(TEntityDto dto);
        Task<bool> Update(TEntityDto dto);
        Task<bool> Delete(TEntityDto dto);
        Task<bool> Save();

        Task<int> GetLast();
    }
}
