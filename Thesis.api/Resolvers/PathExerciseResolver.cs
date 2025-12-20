using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thesis.api;
using Thesis.app.Dtos.LearningPath;
using Thesis.data;
using Thesis.data.Data;

namespace Thesis.app.Resolvers
{
    public class PathExerciseResolver : IValueResolver<Exercise, PathExercise, bool>
    {
        private readonly AppDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ReviewPathHelper pathHelper;

        public PathExerciseResolver(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, ReviewPathHelper pathHelper)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.pathHelper = pathHelper;
        }

        public bool Resolve(Exercise source, PathExercise destination, bool destMember, ResolutionContext context)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new InvalidOperationException("Nastąpił błąd po stronie serwera");

            if (pathHelper.IsReviewPath)
                return false;

            var studentId = int.Parse(userIdClaim.Value);

            var studentExercise = dbContext.StudentExercises.AsNoTracking()
                .FirstOrDefault(se => se.StudentId == studentId && se.ExerciseId == source.Id);

            return studentExercise != null && studentExercise.IsCompleted;
        }
    }
}
