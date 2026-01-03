using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;
using Thesis.app;
using Thesis.app.Dtos.Account;
using Thesis.app.Services;
using Thesis.data;

namespace Thesis.api
{

    //TO DO:
    //SYSTEM OSIAGNIEC TRZEBA UZUPELNIC

    //projekt testowy *

    //dodanie redisa => kontener gotowy!

    //nie wiem czy narazie system osiagniec caly zrobimy, moze narazie przydaloby sie uzupelnic chociaz metode do pierwszego zadania (dodac najwyzej nowa odznake) i sprawdzic jak to zadziala

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

            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var jwtOptionSection = builder.Configuration.GetRequiredSection("Jwt");
            builder.Services.Configure<JwtOptions>(jwtOptionSection);

            var azureOptionSection = builder.Configuration.GetRequiredSection("AzureStorage");
            builder.Services.Configure<AzureStorageOptions>(azureOptionSection);

            builder.Services.AddScoped<IAchievementService, AchievementService>();
            builder.Services.AddScoped<IAdaptiveSystemService, AdaptiveSystemService>();

            var azureOptions = new AzureStorageOptions();
            jwtOptionSection.Bind(azureOptions);
            builder.Services.AddSingleton(azureOptions);

            var pathHelper = new ReviewPathHelper();
            builder.Services.AddSingleton(pathHelper);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            builder.Services.AddSingleton<IFileService, FileService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Wpisz JWT w postaci: <token>",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "ThesisApp_";
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                    new UnprocessableEntityObjectResult(context.ModelState);
            });

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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("frontend", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins("http://localhost:5173");
                });
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


            app.UseCors("frontend");


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
