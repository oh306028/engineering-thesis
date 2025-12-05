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
                .Where(p => !p.IsCompleted && p.StudentId == studentId)
                .ToListAsync();

            var hardestExerciseIds = notFinishedExercises
             .Select(p => new
             {
                 p.ExerciseId,  
                 Score =
                     (p.Exercise.Level ?? 1) * 2 +
                     p.Exercise.Points * 1 +
                     p.WrongAnswers * 3 +
                     p.Attempts * 2 +
                     (p.LastAttempt.HasValue ? 1 : 0)
             })
             .OrderByDescending(x => x.Score)
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
