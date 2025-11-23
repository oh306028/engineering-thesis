using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Queries
{
    public class AdminQuery
    {
        public class Users : IRequest<List<User>> 
        {
        
        }

        public class Classes : IRequest<List<Classroom>>
        {

        }

        public class Loggins : IRequest<List<LoginHistory>>
        {

        }

        public class TeacherAttempts : IRequest<List<Teacher>>
        {

        }    

    }

    public class GetTeacherAttemptsHandler : IRequestHandler<AdminQuery.TeacherAttempts, List<Teacher>>, IHandler
    {
        public AppDbContext DbContext { get; set; }     

        public GetTeacherAttemptsHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;  
        }
        public async Task<List<Teacher>> Handle(AdminQuery.TeacherAttempts request, CancellationToken cancellationToken)
        {   

            return await DbContext.Users.OfType<Teacher>().AsNoTracking().Where(p => p.IsAccepted.HasValue && !p.IsAccepted.Value).ToListAsync(cancellationToken);

        }   
    }

    public class GetUsersForAdmin : IRequestHandler<AdminQuery.Users, List<User>>, IHandler
    {
        public AppDbContext DbContext { get; set; } 

        public GetUsersForAdmin(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }   
        public async Task<List<User>> Handle(AdminQuery.Users request, CancellationToken cancellationToken)
        {

            return await DbContext.Users.AsNoTracking().ToListAsync(cancellationToken);

        }
    }

    public class GetClassesForAdmin : IRequestHandler<AdminQuery.Classes, List<Classroom>>, IHandler
    {
        public AppDbContext DbContext { get; set; } 

        public GetClassesForAdmin(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<List<Classroom>> Handle(AdminQuery.Classes request, CancellationToken cancellationToken)
        {

            return await DbContext.Classrooms.Include(p => p.Teacher).AsNoTracking().ToListAsync(cancellationToken);

        }
    }

    public class GetLogginsForAdmin : IRequestHandler<AdminQuery.Loggins, List<LoginHistory>>, IHandler
    {   
        public AppDbContext DbContext { get; set; }

        public GetLogginsForAdmin(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<List<LoginHistory>> Handle(AdminQuery.Loggins request, CancellationToken cancellationToken)
        {

            return await DbContext.LoginHistories.Include(p => p.User).AsNoTracking().ToListAsync(cancellationToken);

        }
    }
}
