using web6.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// appsettings.json ‚©‚çÚ‘±•¶š—ñæ“¾
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// EF Core —p‚Ì DbContext ‚Æ‚µ‚Ä“o˜^
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));


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
