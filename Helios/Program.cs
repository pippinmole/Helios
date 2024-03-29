using AspNetCore.Identity.Mongo;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Helios.Better_Uptime;
using Helios.Better_Uptime.Options;
using Helios.Core;
using Helios.Data.Users;
using Helios.Database;
using Helios.Datadog.Extensions;
using Helios.Helium;
using Helios.MailService;
using Helios.Paypal;
using Helios.Products.Services;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using reCAPTCHA.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.SetupDatadogLogging();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient();

builder.Services.AddDataProtection();
builder.Services.AddAntiforgery();

builder.Services.Configure<HeliumOptions>(builder.Configuration.GetSection(HeliumOptions.Name));
builder.Services.Configure<MailSenderOptions>(builder.Configuration.GetSection(MailSenderOptions.Name));
builder.Services.Configure<PaypalOptions>(builder.Configuration.GetSection(PaypalOptions.Name));
builder.Services.Configure<MailSenderOptions>(builder.Configuration.GetSection(MailSenderOptions.Name));
builder.Services.Configure<UptimeHeartbeatOptions>(builder.Configuration.GetSection(UptimeHeartbeatOptions.Name));

builder.Services.AddRecaptcha(builder.Configuration.GetSection("RecaptchaSettings"));
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<IPaypalDatabase, PaypalDatabase>();
builder.Services.AddSingleton<IHeliumService, HeliumService>();
builder.Services.AddSingleton<IOrderValidator, OrderValidator>();
builder.Services.AddSingleton<IUptimeHeartbeatService, UptimeHeartbeatService>();
builder.Services.AddScoped<IAppUserManager, AppUserManager>();
builder.Services.AddScoped<IMailSender, MailSender>();

builder.Services.AddNotyf(config => {
    config.DurationInSeconds = 6;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

builder.Services.AddCronJob<UptimeCronJob>(options => {
    options.CronExpression = "0 0/5 * 1/1 * ?";
    // options.CronExpression = "*/2 * * * * *";
    options.TimeZoneInfo = TimeZoneInfo.Local;
});

builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole, ObjectId>(
        identity => {
            identity.Password.RequireDigit = false;
            identity.Password.RequiredLength = 8;

            // ApplicationUser settings
            identity.User.RequireUniqueEmail = true;
            identity.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
        },
        mongo => {
            mongo.ConnectionString = builder.Configuration.GetConnectionString("DatabaseConnectionString");
            mongo.UsersCollection = "user_info";
        })
    .AddDefaultTokenProviders();

builder.Services.AddRouting(x => {
    x.LowercaseUrls = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if ( !app.Environment.IsDevelopment() ) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseNotyf();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();