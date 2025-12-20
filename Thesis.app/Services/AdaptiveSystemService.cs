using Microsoft.EntityFrameworkCore;
using Thesis.data;
using Thesis.data.Data;

namespace Thesis.app.Services
{
    public interface IAdaptiveSystemService
    {
        Task<List<Exercise>> GetHardestExercises(int studentId);
    }

    public class AdaptiveSystemService : IAdaptiveSystemService
    {
        public AppDbContext DbContext { get; }
        public AdaptiveSystemService(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<Exercise>> GetHardestExercises(int studentId)
        {
            var notFinishedExercises = await DbContext.StudentExercises
                .Include(p => p.Student)
                .Include(p => p.Exercise)
                .Where(p => p.StudentId == studentId)
                .ToListAsync();

            var now = DateTime.UtcNow;

            var hardestExerciseIds = notFinishedExercises
                .Select(p =>
                {
                    var baseScore = (p.Exercise.Level ?? 1) * 2 +
                                       p.Exercise.Points * 1 +
                                       p.WrongAnswers * 3 +
                                       p.Attempts * 2;

                    double timePenalty = 0;
                    if (p.LastAttempt.HasValue)
                    {
                        var minutesSinceLastAttempt = (now - p.LastAttempt.Value).TotalMinutes;

                        timePenalty = 1000 / (minutesSinceLastAttempt + 1);
                    }

                    return new
                    {
                        p.ExerciseId,
                        FinalScore = baseScore - timePenalty
                    };
                })
                .OrderByDescending(x => x.FinalScore)
                .Take(5)
                .Select(p => p.ExerciseId)
                .ToList();

            var exercises = await DbContext.Exercises
                .Where(p => hardestExerciseIds.Contains(p.Id))
                .ToListAsync();

            return exercises;
        }
    }
}
