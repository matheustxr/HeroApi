using FluentAssertions;
using HeroApi.Data;
using HeroApi.Entities;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Tests;

public class GetHeroByIdAsyncTests
{
    private async Task<HeroContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<HeroContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new HeroContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task GetHeroByIdAsync_WithValidId_ShouldReturnHero()
    {
        var context = await GetDbContext();
        var hero = new Hero { Id = 1, HeroName = "Superman" };
        context.Heroes.Add(hero);
        await context.SaveChangesAsync();
        var heroService = new HeroService(context);

        var result = await heroService.GetHeroByIdAsync(1);

        result.Should().NotBeNull();
        result?.HeroName.Should().Be("Superman");
    }

    [Fact]
    public async Task GetHeroByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        var context = await GetDbContext();
        var heroService = new HeroService(context);

        var result = await heroService.GetHeroByIdAsync(99);

        result.Should().BeNull();
    }
}