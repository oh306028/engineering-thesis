using Microsoft.EntityFrameworkCore;
using Thesis.data.Data;

namespace Thesis.api.Extensions
{
    public static class ExerciseExtensions
    {
        public static IQueryable<Exercise> WithAllIncludes(this IQueryable<Exercise> query)
        {
            return query
                .Include(e => e.Answer)
                .Include(e => e.Subject)
                .Include(e => e.LearningPathExercises).ThenInclude(lp => lp.LearningPath)
                .Include(e => e.StudentExercises).ThenInclude(p => p.Student);
        }
    }
}
