using web6.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// appsettings.json ����ڑ�������擾
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// EF Core �p�� DbContext �Ƃ��ēo�^
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));

// �Z�b�V������MVC�T�[�r�X�̓o�^
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
