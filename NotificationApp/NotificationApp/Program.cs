using BLL;
using DAL;
using DAL.Interfaces;
using Service;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddAuthentication("AuthCookie") //Implement this
    .AddCookie("AuthCookie", options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.LoginPath = "/access/sign-in";
        options.LogoutPath = "/access/sign-out";
        options.AccessDeniedPath = "/access/denied";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();

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

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//DATABASE TESTING---------------------------------------------
//AccountRepository ar = new AccountRepository();
//AccountService accountService = new AccountService(ar);
//accountService.SignUp("manager", "manager@gmail.com", "test", 1, 2);
//DATABASE TESTING---------------------------------------------

app.Run();


