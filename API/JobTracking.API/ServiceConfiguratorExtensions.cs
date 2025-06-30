using System.Text;
using JobTracking.Application.Contracts;
using JobTracking.Application.Contracts.Base;
using JobTracking.Application.Implementations;
using JobTracking.DataAccess.Persistence;
using JobTracking.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JobTracking.API
{
    public static class ServiceConfiguratorExtensions
    {
        public static void AddContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<DbContext>();

            builder.Services.AddDbContext<AppDbContext/*HrManagementContext*/>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }
 
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("AppDb")
                .AddEntityFrameworkStores<AppDbContext/*HrManagementContext*/>()
                .AddDefaultTokenProviders();
 
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration[Jwt.Issuer],
                    ValidAudience = builder.Configuration[Jwt.Audience],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Jwt.Key] ?? string.Empty))
                };
            });
        }
 
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJobListingService, JobListingService>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<DependencyProvider>();
        }
 
        public static void AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    config =>
                    {
                        config.WithOrigins("http://localhost:4200", "https://localhost:7184")
                            .AllowAnyHeader()
                            .WithMethods(HttpMethod.Get.Method, HttpMethod.Post.Method, HttpMethod.Put.Method,
                                HttpMethod.Delete.Method);
                    });
            });
        }
    }
}