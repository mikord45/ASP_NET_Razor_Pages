using ASP_NET_Razor_Pages.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options => builder.Services.AddDatabaseDeveloperPageExceptionFilter());
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
options => {
    options.Stores.MaxLengthForKeys = 128;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddRoles<IdentityRole>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);

    var context = services.GetRequiredService<ApplicationDbContext>();
    var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

    IdentitySeedData.Initialize(context, userMgr, roleMgr).Wait();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization("pl-PL");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
