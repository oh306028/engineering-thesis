using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Commands
{
    public class ClassroomCommand
    {
        public class CreateClassroom : IRequest<Unit>   
        {
            public ClassroomCreateModel Model { get; set; }
            public int TeacherId { get; set; }  
            public CreateClassroom(ClassroomCreateModel model, int teacherId)
            {
                Model = model;
                TeacherId = teacherId;
            }

        }

        
    }

    public class CreateClassroomHandler : IRequestHandler<ClassroomCommand.CreateClassroom, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public CreateClassroomHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<Unit> Handle(ClassroomCommand.CreateClassroom request, CancellationToken cancellationToken)
        {

            var newClass = new Classroom();
            newClass.ClassName = request.Model.ClassName;
            newClass.DateCreated = DateTime.Now;
            newClass.TeacherId = request.TeacherId;
            newClass.CreatedBy = request.TeacherId;


            newClass.ClassroomKey = GenerateKey();


            DbContext.Classrooms.Add(newClass);
            await DbContext.SaveChangesAsync();
        }

        private string GenerateKey()
        {
            return "str";
        }
    }
}
