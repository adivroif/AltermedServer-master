
using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Services;
using Microsoft.EntityFrameworkCore;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//logging
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(o => {
    o.SingleLine = true;
    o.TimestampFormat = "HH:mm:ss ";
});
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddFilter("Microsoft", LogLevel.Information);
builder.Logging.AddFilter("System", LogLevel.Warning);
builder.Logging.SetMinimumLevel(LogLevel.Information);
//end logging

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("he") };
    options.DefaultRequestCulture = new RequestCulture("he");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});



//For firebase
var firebaseCredentialsPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Firebase:CredentialsFile"]);
FirebaseApp.Create(new AppOptions()
    {
    Credential = GoogleCredential.FromFile(firebaseCredentialsPath)
    });

//***********PRODUCTION DOCKER***************
/*
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); // Optional: keep HTTP for testing

    serverOptions.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps("https/aspnetapp.pfx", "1234");
    });
});
*/
//***********PRODUCTION***************

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register all services in the system
builder.Services.AddScoped<AltermedManager.Services.AppointmentService>();
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddScoped<TreatmentService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<PatientsController>();
builder.Services.AddScoped<PatientsFeedbacksService>();
builder.Services.AddScoped<FeedbackAnalysisServer>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();
//Add HttpClient foe connection to the Health Provider Server
builder.Services.AddHttpClient("DummyProvider", client =>
{
    client.BaseAddress = new Uri("http://localhost:5047");
});
builder.Services.AddScoped<DummyHealthProviderService>();


//for smartphone testing use this port
builder.WebHost.UseUrls("http://0.0.0.0:5000"); // Optional: keep HTTP for testing

var app = builder.Build();


// Configure the HTTP request pipeline.

    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();


//app.UseHttpsRedirection();
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions?.Value);

app.UseAuthorization();

app.MapControllers();

app.Run();
