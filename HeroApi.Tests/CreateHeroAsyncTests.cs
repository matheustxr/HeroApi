using FluentAssertions;
using HeroApi.Data;
using HeroApi.DTOs;
using HeroApi.Entities;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

namespace HeroApi.Tests;

public class CreateHeroAsyncTests
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
            context.Superpowers.AddRange(
                new Superpower { Id = 1, SuperPower = "Voo" },
                new Superpower { Id = 2, SuperPower = "Super Força" }
            );
            await context.SaveChangesAsync();
        }

        return context;
    }

    [Fact]
    public async Task CreateHeroAsync_WithValidData_ShouldCreateHeroSuccessfully()
    {
        var context = await GetDbContext();
        var heroService = new HeroService(context);
        var request = new RequestHeroJson
        {
            Name = "Clark Kent",
            HeroName = "Superman",
            BirthDate = new DateTime(1938, 4, 18),
            Height = 1.91,
            Weight = 107,
            SuperpowerIds = new List<int> { 1, 2 }
        };

        var result = await heroService.CreateHeroAsync(request);

        result.Should().NotBeNull();
        result.HeroName.Should().Be("Superman");
        (await context.Heroes.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task CreateHeroAsync_WithDuplicateHeroName_ShouldReturnNull()
    {
        var context = await GetDbContext();
        context.Heroes.Add(new Hero { HeroName = "Batman" });
        await context.SaveChangesAsync();
        var heroService = new HeroService(context);
        var request = new RequestHeroJson { HeroName = "Batman", SuperpowerIds = new List<int> { 2 } };

        var result = await heroService.CreateHeroAsync(request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateHeroAsync_WithInvalidSuperpowerId_ShouldThrowArgumentException()
    {
        var context = await GetDbContext();
        var heroService = new HeroService(context);
        var request = new RequestHeroJson { HeroName = "Spider-Man", SuperpowerIds = new List<int> { 99 } };

        Func<Task> act = async () => await heroService.CreateHeroAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Um ou mais Ids de superpoderes são inválidos.");
    }
}