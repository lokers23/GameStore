
using GameStore.DAL;
using GameStore.DAL.Intefaces;
using GameStore.DAL.Repositories;
using GameStore.Service.Implementations;
using GameStore.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GameStoreContextDB>(x => x.UseSqlServer(connectionString));

builder.Services.AddScoped<IGameAction, GameAction>();
builder.Services.AddScoped<IGameService, GameService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
