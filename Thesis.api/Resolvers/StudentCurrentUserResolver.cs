using AutoMapper;
using Thesis.app.Dtos.Student;
using Thesis.data.Data;
using Thesis.data;
using System.Security.Claims;

namespace Thesis.api.Resolvers
{
    public class StudentCurrentUserResolver : IValueResolver<Student, StudentDetails, bool>
    {
        private readonly AppDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public StudentCurrentUserResolver(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }

        public bool Resolve(Student source, StudentDetails destination, bool destMember, ResolutionContext context)
        {
           return source.Id == int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }   
}
