using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Thesis.app.Dtos.LearningPath;
using Thesis.data;
using Thesis.data.Data;

namespace Thesis.api.Resolvers
{
    public class FinishedPathResolver : IValueResolver<LearningPath, LearningPathDetails, bool>
    {
        private readonly AppDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FinishedPathResolver(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public bool Resolve(LearningPath source, LearningPathDetails destination, bool destMember, ResolutionContext context)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new InvalidOperationException("Nastąpił błąd po stronie serwera");

            var studentId = int.Parse(userIdClaim.Value);

            var learningPathExercises = dbContext.LearningPathExercises
                .AsNoTracking()
                .Where(p => p.LearningPathId == source.Id)
                .Select(p => p.ExerciseId)
                .ToList();

            var studentExercise = dbContext.StudentExercises
                .AsNoTracking()
                .Where(se => se.StudentId == studentId && learningPathExercises.Contains(se.ExerciseId))
                .ToList();

            return learningPathExercises.Count == studentExercise.Count;

        }
    }
}
