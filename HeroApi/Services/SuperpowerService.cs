using HeroApi.Data;
using HeroApi.DTOs;
using HeroApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Services
{
    public class SuperpowerService : ISuperpowerService
    {
        private readonly HeroContext _context;

        public SuperpowerService(HeroContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SuperpowerJson>> GetAllSuperpowersAsync()
        {
            var superpowers = await _context.Superpowers
                .AsNoTracking()
                .Select(s => new SuperpowerJson
                {
                    Id = s.Id,
                    Superpower = s.SuperPower,
                    Description = s.Description
                })
                .ToListAsync();

            return superpowers;
        }
    }
}
