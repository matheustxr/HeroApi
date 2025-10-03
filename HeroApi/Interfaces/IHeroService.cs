using HeroApi.DTOs;

namespace HeroApi.Interfaces
{
    public interface IHeroService
    {
        Task<ResponseHeroJson?> CreateHeroAsync(RequestHeroJson request);
        Task<IEnumerable<ResponseHeroJson>> GetAllHeroesAsync();
        Task<ResponseHeroJson?> GetHeroByIdAsync(int id);
    }
}
