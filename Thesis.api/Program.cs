using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Thesis.app;
using Thesis.data;
using AutoMapper;
using Thesis.app.Dtos.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using FluentValidation.AspNetCore;
using Thesis.app.Services;

namespace Thesis.api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddValidatorsFromAssemblyContaining<AccountRegisterModelValidator>();
            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddDbContext<AppDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AppAssemblyMarker).Assembly);
            });

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var jwtOptionSection = builder.Configuration.GetRequiredSection("Jwt");
            builder.Services.Configure<JwtOptions>(jwtOptionSection);

            var azureOptionSection = builder.Configuration.GetRequiredSection("AzureStorage");
            builder.Services.Configure<AzureStorageOptions>(azureOptionSection);
                    
            var azureOptions = new AzureStorageOptions();
            jwtOptionSection.Bind(azureOptions);
            builder.Services.AddSingleton(azureOptions);    

            builder.Services.AddSingleton<IFileService, FileService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                    new UnprocessableEntityObjectResult(context.ModelState);
            });

            //TO DO:
            // admin pannel:
            //endpoint for accepting teacher registration
            //

            //TO DO:
            //admin has to check and accept that account!
            //

            builder.Services.AddHttpContextAccessor();

            var jwtOptions = new JwtOptions();
            jwtOptionSection.Bind(jwtOptions);
            builder.Services.AddSingleton(jwtOptions);


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwtOptions =>
            {
                var configKey = jwtOptionSection["Key"];
                var key = Encoding.UTF8.GetBytes(configKey);



                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtOptionSection["Issuer"],
                    ValidAudience = jwtOptionSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                };
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await DataSeeder.SeedAsync(context);
            }

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
