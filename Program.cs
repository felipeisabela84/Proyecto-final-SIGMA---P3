using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIGMA_PROJECT.Data;
using SIGMA_PROJECT.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ReporteEstadoService>();
builder.Services.AddScoped<ReporteValidacionService>();
builder.Services.AddScoped<ReporteConsultaService>();
builder.Services.AddScoped<ReporteEstadisticasService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Crear roles si no existen
    string[] roles = { "Ciudadano", "Operador" };
    foreach (var rol in roles)
    {
        if (!await roleManager.RoleExistsAsync(rol))
            await roleManager.CreateAsync(new IdentityRole(rol));
    }

    // Asignar rol Ciudadano
    var usuarioCiudadano = await userManager.FindByEmailAsync("ciudadano@test.com");
    if (usuarioCiudadano != null && !await userManager.IsInRoleAsync(usuarioCiudadano, "Ciudadano"))
    {
        await userManager.AddToRoleAsync(usuarioCiudadano, "Ciudadano");
    }

    // Asignar rol Operador
    var usuarioOperador = await userManager.FindByEmailAsync("operador@test.com");
    if (usuarioOperador != null && !await userManager.IsInRoleAsync(usuarioOperador, "Operador"))
    {
        await userManager.AddToRoleAsync(usuarioOperador, "Operador");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
