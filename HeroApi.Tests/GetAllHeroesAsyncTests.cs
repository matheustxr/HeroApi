using FluentAssertions;
using HeroApi.Data;
using HeroApi.Entities;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Tests;

public class GetAllHeroesAsyncTests
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
    public async Task GetAllHeroesAsync_WhenHeroesExist_ShouldReturnAllHeroes()
    {
        var context = await GetDbContext();
        context.Heroes.AddRange(new Hero { HeroName = "Superman" }, new Hero { HeroName = "Batman" });
        await context.SaveChangesAsync();
        var heroService = new HeroService(context);

        var result = await heroService.GetAllHeroesAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllHeroesAsync_WhenNoHeroesExist_ShouldReturnEmptyList()
    {
        var context = await GetDbContext();
        var heroService = new HeroService(context);

        var result = await heroService.GetAllHeroesAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}