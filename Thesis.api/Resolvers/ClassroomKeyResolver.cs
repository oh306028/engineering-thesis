using AutoMapper;
using System.Security.Claims;
using Thesis.app.Dtos.Classroom;
using Thesis.app.Dtos.LearningPath;
using Thesis.data;
using Thesis.data.Data;

namespace Thesis.api.Resolvers
{
    public class ClassroomKeyResolver : IValueResolver<Classroom, ClassroomDetails, string>
    {
        private readonly AppDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClassroomKeyResolver(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        public string Resolve(Classroom source, ClassroomDetails destination, string destMember, ResolutionContext context)
        {
            var userRole = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            if (userRole == null)
                throw new InvalidOperationException("Nastąpił błąd po stronie serwera");

            if (userRole == "Teacher")
                return source.ClassroomKey;

            return null;

        }
    }
}
