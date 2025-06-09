using BLL;
using DAL;
using Service;
using Service.Interfaces;
using Service.Utility;
using NotificationApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
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
builder.Services.AddServices();
builder.Services.AddSignalR();


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
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Index}/{id?}");

//AccountRepository accRepo = new();
//AccountService accService = new(accRepo);
//accService.SignUp("Manager", "managerone@gmail.com", "manager", 2, 2);

//accService.SignUp("Manager", "managertwo@gmail.com", "manager", 3, 3);

app.Run();