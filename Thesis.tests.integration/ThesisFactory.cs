using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Thesis.app.Services;
using Thesis.data;
using Thesis.data.Data;

public class ThesisFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public Mock<IAchievementService> AchievementMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ThesisTestDb"));

            services.RemoveAll(typeof(IAuthenticationService));
            services.AddScoped(_ => AchievementMock.Object);


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                try
                {
                    DataSeeder.SeedAsync(db).Wait();
                    if (!db.Users.Any(u => u.Id == 1))
                    {
                        var level1 = db.AccountLevels.First();
                        db.Users.Add(new Student
                        {
                            Id = 1,
                            PublicId = Guid.NewGuid(),
                            CurrentPoints = 0,
                            AccountLevelId = level1.Id
                        });
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Błąd podczas seedowania bazy testowej", ex);
                }
            }

            services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", null);
        });
    }
}