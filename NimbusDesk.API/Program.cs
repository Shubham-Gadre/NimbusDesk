using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NimbusDesk.API.Middleware;
using NimbusDesk.Application.Abstraction.Persistence;
using NimbusDesk.Application.Tickets.Close;
using NimbusDesk.Application.Tickets.Create;
using NimbusDesk.Infrastructure.Persistence;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketValidator>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
                builder.Services.AddDbContext<NimbusDeskDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<CreateTicketHandler>();
            builder.Services.AddScoped<CloseTicketHandler>();


            var app = builder.Build();
           
            // Configure the HTTP request pipeline.
            
                app.MapOpenApi();

            app.UseSwagger();
               app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
