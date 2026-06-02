using Microsoft.EntityFrameworkCore;
using SantaClauzer.BL.Repositories;
using SantaClauzer.BL.Services;
using SantaClauzer.Database.Data;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPresentGroupRepository, PresentGroupRepository>();
builder.Services.AddScoped<IPresentGroupService, PresentGroupService>();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

// Map attribute-routed controllers so api/[controller] endpoints are registered
app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
