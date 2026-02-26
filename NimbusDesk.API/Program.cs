using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NimbusDesk.API.Middleware;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Tickets.Assign;
using NimbusDesk.Application.Tickets.Close;
using NimbusDesk.Application.Tickets.Comment;
using NimbusDesk.Application.Tickets.Create;
using NimbusDesk.Application.Tickets.Queries;
using NimbusDesk.Application.Tickets.ReOpen;
using NimbusDesk.Application.Tickets.Update;
using NimbusDesk.Infrastructure.Identity;
using NimbusDesk.Infrastructure.Persistence;
using System.Text;
using Scalar.AspNetCore;

namespace NimbusDesk.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketValidator>();
            builder.Services.AddOpenApi();

            // DbContext Configuration
            builder.Services.AddDbContext<NimbusDeskDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repository & Application Services
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<CreateTicketHandler>();
            builder.Services.AddScoped<CloseTicketHandler>();
            builder.Services.AddScoped<GetTicketsHandler>();
            builder.Services.AddScoped<GetTicketHistoryHandler>();
            builder.Services.AddScoped<ReopenTicketHandler>();
            builder.Services.AddScoped<UpdateTicketHandler>();
            builder.Services.AddScoped<AssignTicketHandler>();
            builder.Services.AddScoped<AddCommentHandler>();
            builder.Services.AddScoped<GetTicketDetailsHandler>();
            builder.Services.AddScoped<ITokenService, TokenService>();



            // Identity Configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<NimbusDeskDbContext>()
            .AddDefaultTokenProviders();

            // Authentication Configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(5), // Allow 5 second clock drift
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };

                // Handle JWT authentication challenges
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsJsonAsync(new { message = "Unauthorized: Invalid or missing token" });
                    }
                };
            });

            // OpenAPI/Scalar Configuration
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            // Middleware order is CRITICAL

            // Exception handling first
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // OpenAPI with Scalar UI
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .WithTitle("NimbusDesk API")
                    .WithTheme(ScalarTheme.BluePlanet)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });

            app.UseHttpsRedirection();

            // Authentication & Authorization MUST be in this order
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
