using HeroApi.DTOs;

namespace HeroApi.Interfaces
{
    public interface ISuperpowerService
    {
        Task<IEnumerable<SuperpowerJson>> GetAllSuperpowersAsync();
    }
}
