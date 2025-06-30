using JobTracking.API;
using JobTracking.Application.Contracts;
using JobTracking.Application.Implementations;
using JobTracking.DataAccess.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.AddIdentity();
builder.AddServices();

// Configure SQL Server DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=JobTrackingLocalDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"));

// Register services
builder.AddContext();
// builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<IJobListingService, JobListingService>();
// builder.Services.AddScoped<IApplicationService, ApplicationService>();
// builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Optional: Configure basic role-based authorization (no JWT)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization(); // No authentication middleware needed if not using JWT
app.MapControllers();
app.Run();