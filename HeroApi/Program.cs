using HeroApi.Data;
using HeroApi.Entities;
using HeroApi.Interfaces;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HeroContext>(options =>
    options.UseInMemoryDatabase("HeroDb"));

builder.Services.AddScoped<IHeroService, HeroService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<HeroContext>();
    SeedData(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

void SeedData(HeroContext context)
{
    if (!context.Superpowers.Any())
    {
        context.Superpowers.AddRange(
            new Superpower { SuperPower = "Super For�a", Description = "Capacidade de exercer for�a f�sica acima do normal." },
            new Superpower { SuperPower = "Voo", Description = "Capacidade de voar sem aux�lio mec�nico." },
            new Superpower { SuperPower = "Invisibilidade", Description = "Capacidade de se tornar invis�vel a olho nu." },
            new Superpower { SuperPower = "Telepatia", Description = "Capacidade de ler mentes e projetar pensamentos." },
            new Superpower { SuperPower = "Super Velocidade", Description = "Capacidade de se mover em velocidades extraordin�rias." }
        );
        context.SaveChanges();
    }
}