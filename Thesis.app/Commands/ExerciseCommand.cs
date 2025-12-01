using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.api.Extensions;
using Thesis.app.Dtos.Answer;
using Thesis.app.Events;
using Thesis.app.Exceptions;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;
using static Thesis.app.Commands.ExerciseCommand;

namespace Thesis.app.Commands
{
    public class ExerciseCommand
    {
        public class Answer : IRequest<Unit>
        {
            public AnswerModel Model{ get; set; }
            public string ExercisePublicId { get; set; }
            public int StudentId { get; set; }  
            public Answer(string publicId, AnswerModel model, int studentId)
            {
                Model = model;
                StudentId = studentId;
                ExercisePublicId = publicId;
            }
        }
    }


    public class AnswerHandler : IRequestHandler<ExerciseCommand.Answer, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public IMediator MediatR { get; set; }  

        public AnswerHandler(AppDbContext dbContext, IMediator mediatR)
        {
            DbContext = dbContext;
            this.MediatR = mediatR;
        }
            
        public async Task<Unit> Handle(ExerciseCommand.Answer request, CancellationToken cancellationToken)
        {
            var exercise = await DbContext.Exercises
                .WithAllIncludes()
                .SingleAsync(p => p.PublicId.ToString() == request.ExercisePublicId);

            var answer = exercise.Answer;

            var student = DbContext.Users.OfType<Student>().FirstOrDefault(p => p.Id == request.StudentId);

            var studentExercise = await DbContext.StudentExercises
                 .FirstOrDefaultAsync(se => se.StudentId == request.StudentId && se.ExerciseId == exercise.Id);

            if (studentExercise == null)
            {
                studentExercise = new StudentExercises
                {
                    StudentId = request.StudentId,
                    ExerciseId = exercise.Id,
                    Attempts = 0,
                    WrongAnswers = 0,
                    IsCompleted = false
                };
                DbContext.StudentExercises.Add(studentExercise);
            }

            studentExercise.Attempts++;
            studentExercise.LastAttempt = DateTime.UtcNow;

            try
            {
                if ((!(string.IsNullOrEmpty(request.Model.CorrectOption) && request.Model.CorrectOption.ToLower() != answer.CorrectOption?.ToLower())) || (request.Model.CorrectNumber.HasValue && request.Model.CorrectNumber != answer?.CorrectNumber) || 
                    (!string.IsNullOrEmpty(request.Model.CorrectText) && request.Model.CorrectText != answer.CorrectText))
                {
                    studentExercise.WrongAnswers++;
                    throw new WrongAnswerException("Błędna odpowiedź");
                }

                studentExercise.IsCompleted = true;
                student.CurrentPoints += 5;

                await MediatR.Publish(new PointsAddedEvent(student.Id, 5, student.CurrentPoints), cancellationToken);
            }
            catch (WrongAnswerException)
            {
                throw;
            }
            finally
            {
                await DbContext.SaveChangesAsync(cancellationToken);

            }

            return Unit.Value;

        }
    }

}
