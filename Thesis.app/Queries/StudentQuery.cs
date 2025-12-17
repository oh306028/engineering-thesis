using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Student;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Queries
{
    public class StudentQuery
    {
        public class GetProgress : IRequest<StudentProgressDetails>
        {
            public int StudentId { get; set; }
            public int CurrentLevel { get; set; }
            public GetProgress(int studentId, int currentLevel)
            {
                StudentId = studentId;
                CurrentLevel = currentLevel;
            }

        }

        public class GetStudentInfoForParent : IRequest<Student>    
        {
            public int ParentId { get; set; }   
            public GetStudentInfoForParent(int parentId)
            {
                ParentId = parentId;
            }

        }
    }

    public class GetStudentProgressHandler : IHandler, IRequestHandler<StudentQuery.GetProgress, StudentProgressDetails>
    {
        public AppDbContext DbContext { get; set; }

        public GetStudentProgressHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<StudentProgressDetails> Handle(StudentQuery.GetProgress request, CancellationToken cancellationToken)
        {
            var student = await DbContext.Users.OfType<Student>().Include(p => p.AccountLevel).FirstOrDefaultAsync(p => p.Id == request.StudentId, cancellationToken);

            return new StudentProgressDetails()
            {
                CurrentPoints = student.CurrentPoints,
                Level = student.AccountLevel.Level,
                MaxLevelPoints = student.AccountLevel.MaxPoints,
                MinLevelPoints = student.AccountLevel.MinPoints,
                NewLevel = student.AccountLevel.Level == (request.CurrentLevel != 0 ? request.CurrentLevel : student.AccountLevel.Level) ? false : true

            };

        }
    }

    public class GetStudentInfoForParentHandler : IHandler, IRequestHandler<StudentQuery.GetStudentInfoForParent, Student>
    {
        public AppDbContext DbContext { get; set; } 

        public GetStudentInfoForParentHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<Student> Handle(StudentQuery.GetStudentInfoForParent request, CancellationToken cancellationToken)
        {
            var student = await DbContext.Users.OfType<Student>()
                .Include(p => p.AccountLevel)
                .Include(p => p.LoginHistory)
                .Include(p => p.Parent)
                .Include(p => p.Classroom).ThenInclude(p => p.Teacher)
                .FirstOrDefaultAsync(p => p.Parent.Id == request.ParentId, cancellationToken);

            return student;
        }
    }
}
