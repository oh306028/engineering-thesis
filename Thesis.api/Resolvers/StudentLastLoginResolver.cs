using AutoMapper;
using System.Security.Claims;
using Thesis.app.Dtos.Student;
using Thesis.data;
using Thesis.data.Data;

namespace Thesis.api.Resolvers
{
    public class StudentLastLoginResolver : IValueResolver<Student, StudentDetails, DateTime>
    {
        private readonly AppDbContext dbContext;
    private readonly IHttpContextAccessor httpContextAccessor;

    public StudentLastLoginResolver(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        this.dbContext = dbContext;
        this.httpContextAccessor = httpContextAccessor;
    }
        public DateTime Resolve(Student source, StudentDetails destination, DateTime destMember, ResolutionContext context)
        {
            return source.LoginHistory.Where(p => p.UserId == source.Id && p.IsSucceeded).OrderByDescending(p => p.LoginDate).Select(p => p.LoginDate).First();
            
        }
    }
}
