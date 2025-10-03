using HeroApi.DTOs;
using HeroApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHero([FromBody] RequestHeroJson request)
        {
            try
            {
                var newHero = await _heroService.CreateHeroAsync(request);

                if (newHero is null)
                {
                    return Conflict("Já existe um herói cadastrado com esse nome.");
                }

                return CreatedAtAction(nameof(GetHeroById), new { id = newHero.Id }, newHero);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHeroes()
        {
            var heroes = await _heroService.GetAllHeroesAsync();

            if (!heroes.Any())
            {
                return NoContent();
            }

            return Ok(heroes);
        }

    }
}