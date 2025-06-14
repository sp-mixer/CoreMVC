using web6.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// appsettings.json ����ڑ�������擾
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// EF Core �p�� DbContext �Ƃ��ēo�^
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
