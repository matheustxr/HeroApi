using FluentAssertions;
using HeroApi.Data;
using HeroApi.Entities;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Tests;

public class DeleteHeroAsyncTests
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
    public async Task DeleteHeroAsync_WithValidId_ShouldReturnTrue()
    {
        var context = await GetDbContext();
        context.Heroes.Add(new Hero { Id = 1, HeroName = "Superman" });
        await context.SaveChangesAsync();
        var heroService = new HeroService(context);

        var result = await heroService.DeleteHeroAsync(1);

        result.Should().BeTrue();
        (await context.Heroes.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task DeleteHeroAsync_WithInvalidId_ShouldReturnFalse()
    {
        var context = await GetDbContext();
        var heroService = new HeroService(context);

        var result = await heroService.DeleteHeroAsync(99);

        result.Should().BeFalse();
    }
}