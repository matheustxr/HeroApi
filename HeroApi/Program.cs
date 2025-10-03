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
            new Superpower { SuperPower = "Super Força", Description = "Capacidade de exercer força física acima do normal." },
            new Superpower { SuperPower = "Voo", Description = "Capacidade de voar sem auxílio mecânico." },
            new Superpower { SuperPower = "Invisibilidade", Description = "Capacidade de se tornar invisível a olho nu." },
            new Superpower { SuperPower = "Telepatia", Description = "Capacidade de ler mentes e projetar pensamentos." },
            new Superpower { SuperPower = "Super Velocidade", Description = "Capacidade de se mover em velocidades extraordinárias." }
        );
        context.SaveChanges();
    }
}