using HeroApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperpowersController : ControllerBase
    {
        private readonly ISuperpowerService _superpowerService;

        public SuperpowersController(ISuperpowerService superpowerService)
        {
            _superpowerService = superpowerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuperpowers()
        {
            try
            {
                var superpowers = await _superpowerService.GetAllSuperpowersAsync();
                return Ok(superpowers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro interno: {ex.Message}");
            }
        }
    }
}
