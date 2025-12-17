using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Classroom;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Enums;
using Thesis.data.Interfaces;


namespace Thesis.app.Queries
{
    public class ClassroomQuery
    {
        public class GetDetails : IRequest<Classroom> 
        {
            public string PublicId { get; set; }
            public GetDetails(string publicId)
            {
                PublicId = publicId;
            }

        }

        public class GetHomeWorkTypesDict : IRequest<List<HomeWorkType>>
        {


        }

        public class GetMineHomeWork : IRequest<List<HomeWork>>
        {
            public int StudentId { get; set; }
            public GetMineHomeWork(int studentId)
            {
                StudentId = studentId;
            }

        }


        public class GetMineClassDetails : IRequest<Classroom>
        {
            public int StudentId { get; set; }  
            public GetMineClassDetails(int studentId)
            {
                StudentId = studentId;    
            }   

        }
        public class GetStudentsForClassroom : IRequest<List<Student>>
        {
            public string PublicId { get; set; }
            public GetStudentsForClassroom(string publicId)
            {
                PublicId = publicId;
            }
        }

        public class GetStudentRequests : IRequest<List<Student>>
        {
            public string PublicId { get; set; }    
            public GetStudentRequests(string publicId)
            {
                PublicId = publicId;
            }   
        }


        public class GetList : IRequest<List<Classroom>>
        {
            public int TeacherId { get; set; }
            public GetList(int teacherId)
            {
                TeacherId = teacherId;
            }

        }

    }

    public class GetHomeWorkTypesDictHandler : IRequestHandler<ClassroomQuery.GetHomeWorkTypesDict, List<HomeWorkType>>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public GetHomeWorkTypesDictHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;  
        }
        public Task<List<HomeWorkType>> Handle(ClassroomQuery.GetHomeWorkTypesDict request, CancellationToken cancellationToken)
        {
            var result = Enum.GetValues(typeof(HomeWorkType)).Cast<HomeWorkType>().ToList();

            return Task.FromResult(result);

        }
    }

    public class GetMineHomeWorkHandler : IRequestHandler<ClassroomQuery.GetMineHomeWork, List<HomeWork>>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public GetMineHomeWorkHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }   
        public async Task<List<HomeWork>> Handle(ClassroomQuery.GetMineHomeWork request, CancellationToken cancellationToken)
        {
            var student = await DbContext.Users.OfType<Student>()
                .Include(p => p.Classroom).ThenInclude(p => p.HomeWorks).ThenInclude(p => p.Exercises)
                .FirstOrDefaultAsync(p => p.Id == request.StudentId);

            return student.Classroom.HomeWorks.OrderByDescending(p => p.DateCreated).ToList();
        }
    }

    public class GetClassroomDetailsHandler : IRequestHandler<ClassroomQuery.GetDetails, Classroom>, IHandler
    {
        public AppDbContext DbContext { get; set; } 
        public GetClassroomDetailsHandler(AppDbContext dbContext)
        {   
            this.DbContext = dbContext;
        }   

        public async Task<Classroom> Handle(ClassroomQuery.GetDetails request, CancellationToken cancellationToken)
        {
            return await DbContext.Classrooms
                .Include(p => p.Teacher)
                .FirstOrDefaultAsync(p => p.PublicId.ToString() == request.PublicId);
        }   
    }

    public class GetMineClassroomDetailsHandler : IRequestHandler<ClassroomQuery.GetMineClassDetails, Classroom>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public GetMineClassroomDetailsHandler(AppDbContext dbContext)
        {
            this.DbContext = dbContext;
        }   

        public async Task<Classroom> Handle(ClassroomQuery.GetMineClassDetails request, CancellationToken cancellationToken)
        {
            var student = await DbContext.Users.OfType<Student>()
                .Include(p => p.Classroom).ThenInclude(p => p.Teacher)
                .FirstOrDefaultAsync(p => p.Id == request.StudentId, cancellationToken);

            return student.IsAcceptedToClass ? student.Classroom : null;
        }
    }

    public class GetStudentsForClassroomHandler : IRequestHandler<ClassroomQuery.GetStudentsForClassroom, List<Student>>, IHandler
    {   
        public AppDbContext DbContext { get; set; } 
        public GetStudentsForClassroomHandler(AppDbContext dbContext)   
        {
            this.DbContext = dbContext;
        }

        public async Task<List<Student>> Handle(ClassroomQuery.GetStudentsForClassroom request, CancellationToken cancellationToken)
        {
            var classroom = await DbContext.Classrooms
                .Include(p => p.Students).ThenInclude(p => p.LoginHistory)
                .Include(p => p.Students).ThenInclude(p => p.AccountLevel)
                .Include(p => p.Students).ThenInclude(p => p.StudentBadges)
                .FirstOrDefaultAsync(p => p.PublicId.ToString() == request.PublicId);

            return classroom.Students.Where(p => p.IsAcceptedToClass).OrderByDescending(p => p.CurrentPoints).ToList();

        }
    }

    public class GetStudentRequestsHandler : IRequestHandler<ClassroomQuery.GetStudentRequests, List<Student>>, IHandler
    {   
        public AppDbContext DbContext { get; set; }
        public GetStudentRequestsHandler(AppDbContext dbContext)
        {
            this.DbContext = dbContext; 
        }

        public async Task<List<Student>> Handle(ClassroomQuery.GetStudentRequests request, CancellationToken cancellationToken)
        {
            var classroom = await DbContext.Classrooms
                .Include(p => p.Students).ThenInclude(p => p.LoginHistory)
                .Include(p => p.Students).ThenInclude(p => p.AccountLevel)
                .Include(p => p.Students).ThenInclude(p => p.StudentBadges)
                .Include(p => p.Teacher)
                .FirstOrDefaultAsync(p => p.PublicId.ToString() == request.PublicId);

            return classroom.Students.Where(p => !p.IsAcceptedToClass).ToList();

        }
    }



    public class GetClassroomListHandler : IRequestHandler<ClassroomQuery.GetList, List<Classroom>>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public GetClassroomListHandler(AppDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<List<Classroom>> Handle(ClassroomQuery.GetList request, CancellationToken cancellationToken)
        {
            return await DbContext.Classrooms.Where(p => p.TeacherId == request.TeacherId).ToListAsync();
        }
    }

}
