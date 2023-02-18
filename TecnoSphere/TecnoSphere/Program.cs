using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TecnoSphere.Areas.Identity.Data;
using TecnoSphere.Core;
using TecnoSphere.Core.Repositories;
using TecnoSphere.Repositories;
using Microsoft.VisualBasic;
using ASP.TecnoSphere.Repositories;
using TecnoSphere.Models.GoogleCaptcha;
using TecnoSphere.Models.EmailSender;
using TecnoSphere.Models.ContactEmailSender;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDBContextConnection");

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>();

//Emaial sender interface 
builder.Services.AddScoped<IMailSender, EmailSender>();
builder.Services.AddScoped<IContactEmailSender, ContactEmailSender>();

//Goggle v3 Captcha interigation
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));

//Use Google Captcha service
builder.Services.AddTransient(typeof(GoogleCaptchaService));



// Add services to the container.
builder.Services.AddControllersWithViews();

#region Authorization

AddAuthorizationPolicies();

#endregion

AddScoped();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();


void AddAuthorizationPolicies()
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("MemberOnly", policy => policy.RequireClaim("MemberNumber"));
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(TecnoSphere.Core.Constants.Policies.RequireAdmin, policy => policy.RequireRole(TecnoSphere.Core.Constants.Roles.Administrator));
        options.AddPolicy(TecnoSphere.Core.Constants.Policies.RequireManager, policy => policy.RequireRole(TecnoSphere.Core.Constants.Roles.Manager));
    });
}

void AddScoped()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
}