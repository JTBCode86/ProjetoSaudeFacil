using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SaudeFacil.Application.Interfaces;
using SaudeFacil.Application.Services;
using SaudeFacil.Domain.Interfaces;
using SaudeFacil.Infrastructure.Data;
using SaudeFacil.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using SaudeFacil.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do DbContext (Entity Framework)
builder.Services.AddDbContext<SaudeFacilDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Injeção de Dependência dos Repositórios (Infrastructure)
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFarmaciaRepository, FarmaciaRepository>();
builder.Services.AddScoped<IRemedioRepository, RemedioRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// 3. Injeção de Dependência dos Serviços (Application)
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IFarmaciaService, FarmaciaService>();
builder.Services.AddScoped<IRemedioService, RemedioService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IImportacaoService, ImportacaoService>();
builder.Services.AddScoped<IPasswordHasher<Farmacia>, PasswordHasher<Farmacia>>();

// 4. Configuração de Autenticação (Cookies para Login de Cliente/Farmácia)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Conta/Login";
        options.AccessDeniedPath = "/Conta/AcessoNegado";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// 5. Configuração de Session (para o Carrinho de Compras)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Executa e popula o banco de dados na inicialização
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SaudeFacilDbContext>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedAsync(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware de Autenticação e Sessão (Ordem Importante!)
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();