using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZayShop_Dapper_Ui.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ZayShopDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ZayShopDbContextConnection' not found.");

builder.Services.AddDbContext<ZayShopDbContext>(options =>
		options.UseSqlServer(connectionString));

builder.Services.AddDbContext<ZayShopDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ZayShopDbContext>();

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Default}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	var roles = new[] { "Admin", "Member" };

	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
			await roleManager.CreateAsync(new IdentityRole(role));
	}
}

app.MapRazorPages();

app.Run();
