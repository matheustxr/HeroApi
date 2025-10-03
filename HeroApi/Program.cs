using HeroApi.Data;
using HeroApi.Entities;
using HeroApi.Interfaces;
using HeroApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HeroContext>(options =>
    options.UseInMemoryDatabase("HeroDb"));

builder.Services.AddScoped<IHeroService, HeroService>();
builder.Services.AddScoped<ISuperpowerService, SuperpowerService>();

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
app.UseCors(MyAllowSpecificOrigins);
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

    if (!context.Heroes.Any())
    {
        var superForca = context.Superpowers.Single(s => s.SuperPower == "Super Força");
        var voo = context.Superpowers.Single(s => s.SuperPower == "Voo");
        var superVelocidade = context.Superpowers.Single(s => s.SuperPower == "Super Velocidade");
        var telepatia = context.Superpowers.Single(s => s.SuperPower == "Telepatia");

        context.Heroes.AddRange(
            new Hero
            {
                Name = "Clark Kent",
                HeroName = "Superman",
                BirthDate = new DateTime(1938, 4, 18),
                Height = 1.91,
                Weight = 107,
                HeroSuperpowers = new List<HeroSuperpower>
                {
                    new HeroSuperpower { Superpower = superForca },
                    new HeroSuperpower { Superpower = voo },
                    new HeroSuperpower { Superpower = superVelocidade }
                }
            },
            new Hero
            {
                Name = "Bruce Wayne",
                HeroName = "Batman",
                BirthDate = new DateTime(1939, 5, 1),
                Height = 1.88,
                Weight = 95
            },
            new Hero
            {
                Name = "Charles Xavier",
                HeroName = "Professor X",
                BirthDate = new DateTime(1932, 8, 20),
                Height = 1.83,
                Weight = 86,
                HeroSuperpowers = new List<HeroSuperpower>
                {
                    new HeroSuperpower { Superpower = telepatia }
                }
            }
        );
        context.SaveChanges();
    }
}