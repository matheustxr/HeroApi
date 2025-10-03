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

        public async Task<ResponseHeroJson?> CreateHeroAsync(RequestHeroJson dto)
        {
            // Validação 1: Verifica se o nome do herói já existe
            if (await _context.Heroes.AnyAsync(h => h.HeroName == dto.HeroName))
            {
                return null; // Conflito, será tratado pelo Controller como HTTP 409
            }

            // Validação 2: Verifica se todos os IDs de superpoderes fornecidos são válidos
            var superpowerCount = await _context.Superpowers
                                    .CountAsync(s => dto.SuperpowerIds.Contains(s.Id));

            if (superpowerCount != dto.SuperpowerIds.Count)
            {
                // Se a contagem for diferente, um ou mais IDs são inválidos.
                // Lança uma exceção que será tratada pelo Controller como HTTP 400.
                throw new ArgumentException("Um ou mais Ids de superpoderes são inválidos.");
            }

            // Cria a entidade Hero
            var hero = new Hero
            {
                Name = dto.Name,
                HeroName = dto.HeroName,
                BirthDate = dto.BirthDate,
                Height = dto.Height,
                Weight = dto.Weight
            };

            // Cria as associações na tabela de junção
            foreach (var superpowerId in dto.SuperpowerIds)
            {
                hero.HeroSuperpowers.Add(new HeroSuperpower
                {
                    SuperpowerId = superpowerId
                    // O HeroId será preenchido automaticamente pelo EF
                });
            }

            // Adiciona o herói e suas associações ao contexto
            _context.Heroes.Add(hero);
            await _context.SaveChangesAsync();

            // Recarrega os dados para o DTO de resposta, garantindo que os nomes dos superpoderes venham juntos
            await _context.Entry(hero).Collection(h => h.HeroSuperpowers).LoadAsync();
            foreach (var hs in hero.HeroSuperpowers)
            {
                await _context.Entry(hs).Reference(s => s.Superpower).LoadAsync();
            }

            // Retorna o DTO de resposta
            return new ResponseHeroJson
            {
                Id = hero.Id,
                Name = hero.Name,
                HeroName = hero.HeroName,
                BirthDate = hero.BirthDate,
                Height = hero.Height,
                Weight = hero.Weight,
                Superpowers = hero.HeroSuperpowers.Select(hs => hs.Superpower.Name).ToList()
            };
        }
    }
}