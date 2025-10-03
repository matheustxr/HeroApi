using FluentAssertions;
using HeroApi.Data;
using HeroApi.DTOs;
using HeroApi.Entities;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Tests;

public class UpdateHeroAsyncTests
{
    private async Task<HeroContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<HeroContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new HeroContext(options);
        context.Database.EnsureCreated();

        if (!await context.Superpowers.AnyAsync())
        {
            context.Superpowers.Add(new Superpower { Id = 1, SuperPower = "Voo" });
            await context.SaveChangesAsync();
        }

        return context;
    }

    [Fact]
    public async Task UpdateHeroAsync_WithValidData_ShouldUpdateAndReturnHero()
    {
        var context = await GetDbContext();
        context.Heroes.Add(new Hero { Id = 1, HeroName = "Superman", Name = "Clark Kent" });
        await context.SaveChangesAsync();
        var heroService = new HeroService(context);
        var request = new RequestHeroJson { Name = "Kal-El", HeroName = "Superman", SuperpowerIds = new List<int> { 1 } };

        var result = await heroService.UpdateHeroAsync(1, request);

        result.Should().NotBeNull();
        result?.Name.Should().Be("Kal-El");
    }

    [Fact]
    public async Task UpdateHeroAsync_WithInvalidId_ShouldReturnNull()
    {
        var context = await GetDbContext();
        var heroService = new HeroService(context);
        var request = new RequestHeroJson { SuperpowerIds = new List<int> { 1 } };

        var result = await heroService.UpdateHeroAsync(99, request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateHeroAsync_WithDuplicateHeroName_ShouldThrowInvalidOperationException()
    {
        var context = await GetDbContext();
        context.Heroes.AddRange(new Hero { Id = 1, HeroName = "Superman" }, new Hero { Id = 2, HeroName = "Batman" });
        await context.SaveChangesAsync();
        var heroService = new HeroService(context);
        var request = new RequestHeroJson { HeroName = "Batman", SuperpowerIds = new List<int> { 1 } };

        Func<Task> act = async () => await heroService.UpdateHeroAsync(1, request);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}