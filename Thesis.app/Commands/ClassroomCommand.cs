using MediatR;
using Microsoft.EntityFrameworkCore;
using Thesis.app.Dtos.Classroom;
using Thesis.app.Dtos.HomeWork;
using Thesis.app.Events;
using Thesis.app.Exceptions;
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

        public class CreateHomeWork : IRequest<Unit>
        {
            public HomeWorkModel Model { get; set; }
            public int TeacherId { get; set; }
            public string ClassroomId { get; set; }
            public CreateHomeWork(HomeWorkModel model, int teacherId, string classroomId)
            {
                Model = model;  
                TeacherId = teacherId;
                ClassroomId = classroomId;
            }   

        }

        public class JoinClassroom : IRequest<Unit>
        {
            public JoinClassroomModel Model { get; set; }   
            public int StudentId { get; set; }  

            public JoinClassroom(JoinClassroomModel model, int studentId)
            {
                Model = model;
                StudentId = studentId;  
            }   
                
        }

        public class AcceptStudent : IRequest<Unit>
        {
            public string StudentPublicId { get; set; }
            public string ClassroomId { get; set; }
            public int TeacherId { get; set; }
            public bool IsAccepted { get; }

            public AcceptStudent(string classroomId, string studentPublicId, int teacherId, bool isAccepted)
            {
                ClassroomId = classroomId;
                StudentPublicId = studentPublicId;
                TeacherId = teacherId;
                IsAccepted = isAccepted;
            }

        }


    }
    public class CreateHomeWork : IRequestHandler<ClassroomCommand.CreateHomeWork, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public IMediator MediatR { get; }

        public CreateHomeWork(AppDbContext dbContext, IMediator mediatR)
        {
            DbContext = dbContext;
            MediatR = mediatR;
        }

        public async Task<Unit> Handle(ClassroomCommand.CreateHomeWork request, CancellationToken cancellationToken)
        {

            var classroom = DbContext.Classrooms.FirstOrDefault(p => p.PublicId == Guid.Parse(request.ClassroomId));

            var homeWork = new HomeWork()
            {
                ClassroomId = classroom.Id,
                Type = (int)request.Model.Type,
                DateCreated = DateTime.Now,
                DeadLine = request.Model.DeadLine,
                TeacherId = request.TeacherId,
                DateModified = DateTime.Now,
                Subject = request.Model.Subject,
                Title = request.Model.Title,
                Description = request.Model.Description

            };

            request.Model.Exercises.ForEach(p =>
            {
               homeWork.Exercises.Add(new Exercise(){ DateCreated = DateTime.Now, Content = p.Content, CreatedBy = request.TeacherId });
            });

            DbContext.HomeWorks.Add(homeWork);
            await DbContext.SaveChangesAsync();

            await MediatR.Publish(new CreatedHomeWorkEvent(classroom.Id));

            return Unit.Value;
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

            return Unit.Value;
        }

        private string GenerateKey()
        {
            var random = new Random();
            char[] letters = new char[3];

            for (int i = 0; i < 3; i++)
            {
                letters[i] = (char)random.Next('A', 'Z' + 1);
            }

            char[] digits = new char[4];
            for (int i = 0; i < 4; i++)
            {
                digits[i] = (char)random.Next('0', '9' + 1);
            }

            return new string(letters) + new string(digits);
        }
    }

    public class JoinClassroomHandler : IRequestHandler<ClassroomCommand.JoinClassroom, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }
            
        public JoinClassroomHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<Unit> Handle(ClassroomCommand.JoinClassroom request, CancellationToken cancellationToken)
        {
            var classroom = await DbContext.Classrooms
            .Include(p => p.Students)
            .FirstOrDefaultAsync(p => p.ClassroomKey == request.Model.ClassroomKey);

            if (classroom == null)
                throw new NotFoundException("Błędny kod klasy");

            var student = (Student)DbContext.Users.Single(p => p.Id == request.StudentId);

            if (student.IsAppendingToClass)
                throw new NotFoundException("Wysłano już prośbę o dołączenie do innej klasy");

            student.IsAppendingToClass = true;  

            classroom.Students.Add(student);
            await DbContext.SaveChangesAsync();

            return Unit.Value;

        }
      
    }

    public class AcceptStudentHandler : IRequestHandler<ClassroomCommand.AcceptStudent, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public AcceptStudentHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<Unit> Handle(ClassroomCommand.AcceptStudent request, CancellationToken cancellationToken)
        {
      
            if (request.IsAccepted)
            {
                var student = (Student)DbContext.Users.Single(p => p.PublicId.ToString() == request.StudentPublicId);

                student.IsAcceptedToClass = true;
                student.IsAppendingToClass = false;

            }
            else
            {
                var classroom = DbContext.Classrooms
                .Include(p => p.Students)
                .Single(p => p.PublicId.ToString() == request.ClassroomId);

                var student = (Student)DbContext.Users.Single(p => p.PublicId.ToString() == request.StudentPublicId);
                student.IsAcceptedToClass = false;
                student.IsAppendingToClass = false;

                classroom.Students.Remove(student);

            }

            await DbContext.SaveChangesAsync();

            return Unit.Value;
        }

    }
}
