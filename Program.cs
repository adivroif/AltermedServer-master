
using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Services;
using Microsoft.EntityFrameworkCore;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//For firebase
var firebaseCredentialsPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Firebase:CredentialsFile"]);
FirebaseApp.Create(new AppOptions()
    {
    Credential = GoogleCredential.FromFile(firebaseCredentialsPath)
    });

//***********PRODUCTION***************
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

builder.WebHost.UseUrls("http://0.0.0.0:5000"); // Optional: keep HTTP for testing

var app = builder.Build();


// Configure the HTTP request pipeline.

    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
