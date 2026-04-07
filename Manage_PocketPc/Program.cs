using Manage_PocketPc.Models;
using Manage_PocketPc.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ICsvImporter, CsvImporter>();

// 1) Thêm Authentication + Authorization (Cookie)
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Account";       // Trang đăng nhập của bạn
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

// 2) Đăng ký MVC và ép yêu cầu đăng nhập mặc định cho toàn bộ controller
builder.Services.AddControllersWithViews(options =>
{
    // Mặc định mọi action đều yêu cầu đăng nhập
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

var app = builder.Build();

// 3) Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // ⚠️ BẮT BUỘC: trước UseAuthorization
app.UseAuthorization();

// 4) Route mặc định (để về trang login khi vào root)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Account}/{id?}");

app.Run();