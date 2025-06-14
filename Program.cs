using web6.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// appsettings.json から接続文字列取得
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// EF Core 用の DbContext として登録
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
