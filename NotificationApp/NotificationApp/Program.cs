using BLL;
using DAL;
using Service;

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
builder.Services.AddScoped<IRepository<Account>, AccountRepository>();

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
//AccountService accountService = new AccountService();
//accountService.SignUp("admin", "admin@gmail.com", "admin", 1);
//DATABASE TESTING---------------------------------------------

app.Run();


