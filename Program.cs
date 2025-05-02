
using AltermedManager.Controllers;
using AltermedManager.Data;
using AltermedManager.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register all services in the system
builder.Services.AddScoped<AltermedManager.Services.AppointmentService>();
builder.Services.AddScoped<RecommendationService>();
builder.Services.AddScoped<TreatmentService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<PatientsController>();
builder.Services.AddScoped<PatientsFeedbacksService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
