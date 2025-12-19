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

namespace Thesis.app.Commands
{
    public class StudentCommand
    {
        public class SetFilters : IRequest<Unit>
        {
            public int ParentId { get; set; }
            public StudentFilterModel Model { get; set; }

            public SetFilters(StudentFilterModel model, int parentId)
            {
                Model = model;
                ParentId = parentId;
            }
        }

    }

    public class StudentCommandHandler : IRequestHandler<StudentCommand.SetFilters, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public StudentCommandHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Unit> Handle(StudentCommand.SetFilters request, CancellationToken cancellationToken)
        {
            var student = await DbContext.Users.OfType<Student>()
                .Include(p => p.Parent)
                .Include(p => p.StudentFilter)
                .FirstOrDefaultAsync(p => p.ParentId == request.ParentId, cancellationToken);

            var subject = await DbContext.Subjects.FirstOrDefaultAsync(p => p.PublicId == request.Model.SubjectId);

            if(student.StudentFilter == null)
            {
                student.StudentFilter = new StudentFilter()
                {
                    Subject = subject,
                    Level = request.Model.Level,
                    DateSet = DateTime.Now
                };

            }
            else
            {
                student.StudentFilter.Subject = subject;
                student.StudentFilter.Level = request.Model.Level;
                student.StudentFilter.DateSet = DateTime.Now;
            }


            await DbContext.SaveChangesAsync();
            return Unit.Value;
        }   
    }
}
