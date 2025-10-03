using HeroApi.Data;
using HeroApi.DTOs;
using HeroApi.Entities;
using HeroApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Services
{
    public class HeroService : IHeroService
    {
        private readonly HeroContext _context;

        public HeroService(HeroContext context)
        {
            _context = context;
        }

        public async Task<ResponseHeroJson?> CreateHeroAsync(RequestHeroJson request)
        {
            if (await _context.Heroes.AnyAsync(h => h.HeroName == request.HeroName))
            {
                return null;
            }

            var superpowerCount = await _context.Superpowers
                                    .CountAsync(s => request.SuperpowerIds.Contains(s.Id));

            if (superpowerCount != request.SuperpowerIds.Count)
            {
                throw new ArgumentException("Um ou mais Ids de superpoderes são inválidos.");
            }

            var hero = new Hero
            {
                Name = request.Name,
                HeroName = request.HeroName,
                BirthDate = request.BirthDate,
                Height = request.Height,
                Weight = request.Weight
            };

            foreach (var superpowerId in request.SuperpowerIds)
            {
                hero.HeroSuperpowers.Add(new HeroSuperpower
                {
                    SuperpowerId = superpowerId
                });
            }

            _context.Heroes.Add(hero);
            await _context.SaveChangesAsync();

            await _context.Entry(hero).Collection(h => h.HeroSuperpowers).LoadAsync();
            foreach (var hs in hero.HeroSuperpowers)
            {
                await _context.Entry(hs).Reference(s => s.Superpower).LoadAsync();
            }

            return new ResponseHeroJson
            {
                Id = hero.Id,
                Name = hero.Name,
                HeroName = hero.HeroName,
                BirthDate = hero.BirthDate,
                Height = hero.Height,
                Weight = hero.Weight,
                Superpowers = hero.HeroSuperpowers.Select(hs => hs.Superpower.SuperPower).ToList()
            };
        }

        public async Task<IEnumerable<ResponseHeroJson>> GetAllHeroesAsync()
        {
            var heroes = await _context.Heroes
                .Include(h => h.HeroSuperpowers)
                .ThenInclude(hs => hs.Superpower)
                .AsNoTracking()
                .ToListAsync();

            return heroes.Select(hero => new ResponseHeroJson
            {
                Id = hero.Id,
                Name = hero.Name,
                HeroName = hero.HeroName,
                BirthDate = hero.BirthDate,
                Height = hero.Height,
                Weight = hero.Weight,
                Superpowers = hero.HeroSuperpowers.Select(hs => hs.Superpower.SuperPower).ToList()
            });
        }

        public async Task<ResponseHeroJson?> GetHeroByIdAsync(int id)
        {
            var hero = await _context.Heroes
                .Include(h => h.HeroSuperpowers)
                .ThenInclude(hs => hs.Superpower)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hero is null)
            {
                return null; 
            }

            return new ResponseHeroJson
            {
                Id = hero.Id,
                Name = hero.Name,
                HeroName = hero.HeroName,
                BirthDate = hero.BirthDate,
                Height = hero.Height,
                Weight = hero.Weight,
                Superpowers = hero.HeroSuperpowers.Select(hs => hs.Superpower.SuperPower).ToList()
            };
        }
        
        public async Task<ResponseHeroJson?> UpdateHeroAsync(int id, RequestHeroJson request)
        {
            var hero = await _context.Heroes
                .Include(h => h.HeroSuperpowers)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hero is null)
            {
                return null;
            }

            if (await _context.Heroes.AnyAsync(h => h.HeroName == request.HeroName && h.Id != id))
            {
                throw new InvalidOperationException("O nome de herói informado já está em uso por outro herói.");
            }

            var superpowerCount = await _context.Superpowers.CountAsync(s => request.SuperpowerIds.Contains(s.Id));
            if (superpowerCount != request.SuperpowerIds.Count)
            {
                throw new ArgumentException("Um ou mais Ids de superpoderes são inválidos.");
            }

            hero.Name = request.Name;
            hero.HeroName = request.HeroName;
            hero.BirthDate = request.BirthDate;
            hero.Height = request.Height;
            hero.Weight = request.Weight;

            hero.HeroSuperpowers.Clear();
            foreach (var superpowerId in request.SuperpowerIds)
            {
                hero.HeroSuperpowers.Add(new HeroSuperpower { SuperpowerId = superpowerId });
            }

            await _context.SaveChangesAsync();

            return await GetHeroByIdAsync(id);
        }

    }
}