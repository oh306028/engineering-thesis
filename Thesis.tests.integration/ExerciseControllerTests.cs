using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Thesis.api;
using Thesis.app.Dtos.Answer;
using Thesis.data;
using Thesis.data.Data;
using Xunit;

public class ExerciseIntegrationTests : IClassFixture<ThesisFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ThesisFactory<Program> _factory;

    public ExerciseIntegrationTests(ThesisFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostAnswer_Should_AddPoints_When_AnswerIsCorrect()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        db.Database.EnsureDeleted();

        var student = new Student { Id = 1, CurrentPoints = 0, PublicId = Guid.NewGuid() };
        var exercise = new Exercise
        {
            Id = 100,
            PublicId = Guid.NewGuid(),
            Level = 2,
            Answer = new Answer { CorrectText = "Kraków" }
        };

        db.Users.Add(student);
        db.Exercises.Add(exercise);
        await db.SaveChangesAsync();

        var payload = new AnswerModel { CorrectText = "Kraków" };

        var response = await _client.PostAsJsonAsync($"/api/exercise/{exercise.PublicId}/answer", payload);

        response.IsSuccessStatusCode.Should().BeTrue();

        var updatedStudent = await db.Users.OfType<Student>().FirstAsync(s => s.Id == 1);
        updatedStudent.CurrentPoints.Should().Be(10);
    }

    [Fact]
    public async Task PostAnswer_Should_ThrowError_When_AnswerIsWrong()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var exPublicId = Guid.NewGuid();
        db.Exercises.Add(new Exercise
        {
            PublicId = exPublicId,
            Answer = new Answer { CorrectText = "Prawda" }
        });
        await db.SaveChangesAsync();

        var payload = new AnswerModel { CorrectText = "Fałsz" };

        var response = await _client.PostAsJsonAsync($"/api/exercise/{exPublicId}/answer", payload);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.UnprocessableEntity);

        var studentExercise = await db.StudentExercises.FirstOrDefaultAsync(se => se.ExerciseId == 100);
    }

    [Fact]
    public async Task PostAnswer_Should_Work_For_Seeded_Exercise()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var exercise = await db.Exercises.FirstAsync(e => e.Content.Contains("5 x 7"));
        var payload = new AnswerModel { CorrectNumber = 35 };

        var response = await _client.PostAsJsonAsync($"/api/exercise/{exercise.PublicId}/answer", payload);

        response.EnsureSuccessStatusCode();

        var student = await db.Users.OfType<Student>().FirstAsync(u => u.Id == 1);
        student.CurrentPoints.Should().BeGreaterThan(0);
    }
}