using AspNetCore.Identity.Mongo;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using FluentEmail.Mailgun;
using Helios.Core;
using Helios.Data.Users;
using Helios.Database;
using Helios.Helium;
using Helios.MailService;
using Helios.Paypal;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using reCAPTCHA.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDataProtection();
builder.Services.AddAntiforgery();

builder.Services.Configure<MailSenderOptions>(builder.Configuration.GetSection(MailSenderOptions.Name));
builder.Services.Configure<PaypalOptions>(builder.Configuration.GetSection(PaypalOptions.Name));

var mailgunOptions = new MailSenderOptions();
builder.Configuration.GetSection(MailSenderOptions.Name).Bind(mailgunOptions);

builder.Services
    .AddFluentEmail($"{mailgunOptions.FromName}@{mailgunOptions.Domain}", "mailgun")
    .AddRazorRenderer($"{Directory.GetCurrentDirectory()}/Email Templates")
    .AddMailGunSender(mailgunOptions.Domain, mailgunOptions.ApiKey, MailGunRegion.EU);

builder.Services.AddRecaptcha(builder.Configuration.GetSection("RecaptchaSettings"));
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<IPaypalDatabase, PaypalDatabase>();
builder.Services.AddSingleton<IHeliumService, HeliumService>();
builder.Services.AddScoped<IAppUserManager, AppUserManager>();
builder.Services.AddScoped<IMailSender, MailSender>();

builder.Services.AddNotyf(config => {
    config.DurationInSeconds = 6;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

builder.Services.AddCronJob<UptimeCronJob>(options => {
    options.CronExpression = "*/20 * * * * *";
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

if ( app.Environment.IsDevelopment() ) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();