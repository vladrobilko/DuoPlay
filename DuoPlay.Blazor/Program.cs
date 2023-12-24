using DuoPlay.Application.Services;
using DuoPlay.Application.Services.Interfaces;
using DuoPlay.Blazor.Components;
using DuoPlay.DataManagement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IBattleShipService, BattleShipService>();
builder.Services.AddScoped<ITicTacToeService, TicTacToeService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")),
    ServiceLifetime.Scoped);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
