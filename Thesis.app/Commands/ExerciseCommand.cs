using MediatR;
using Microsoft.EntityFrameworkCore;
using Thesis.api;
using Thesis.api.Extensions;
using Thesis.app.Dtos.Answer;
using Thesis.app.Events;
using Thesis.app.Exceptions;
using Thesis.app.Services;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Commands
{
    public class ExerciseCommand
    {
        public class Answer : IRequest<Unit>
        {
            public AnswerModel Model { get; set; }
            public string ExercisePublicId { get; set; }
            public int StudentId { get; set; }
            public Answer(string publicId, AnswerModel model, int studentId)
            {
                Model = model;
                StudentId = studentId;
                ExercisePublicId = publicId;
            }
        }

        public class GameAnswer : IRequest<Unit>
        {
            public AnswerModel Model { get; set; }
            public string ExercisePublicId { get; set; }
            public Guid SessionId { get; set; }
            public GameAnswer(string publicId, Guid sessionId, AnswerModel model)
            {
                Model = model;
                ExercisePublicId = publicId;
                SessionId = sessionId;
            }
        }
    }


    public class AnswerHandler : IHandler,
                                IRequestHandler<ExerciseCommand.Answer, Unit>, 
                                IRequestHandler<ExerciseCommand.GameAnswer, Unit>
    {
        public AppDbContext DbContext { get; set; }
        public IMediator MediatR { get; set; }

        public IAchievementService AchievementService { get; set; }
        public ReviewPathHelper PathHelper { get; }

        public AnswerHandler(AppDbContext dbContext, IMediator mediatR, IAchievementService achievementService, ReviewPathHelper pathHelper)
        {
            DbContext = dbContext;
            this.MediatR = mediatR;
            AchievementService = achievementService;
            PathHelper = pathHelper;
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

            if (PathHelper.IsReviewPath)
            {
                studentExercise.Attempts++;
                studentExercise.LastAttempt = DateTime.UtcNow;
                studentExercise.WrongAnswers--;

                try
                {
                    if (!string.IsNullOrEmpty(request.Model.CorrectOption) &&
                    request.Model.CorrectOption.ToLower() != answer.CorrectOption?.ToLower()
                    || (request.Model.CorrectNumber.HasValue && request.Model.CorrectNumber != answer?.CorrectNumber)
                    || (!string.IsNullOrEmpty(request.Model.CorrectText) && request.Model.CorrectText != answer.CorrectText))
                    {
                        studentExercise.WrongAnswers++;
                        throw new WrongAnswerException("Błędna odpowiedź");
                    }

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
                if (!string.IsNullOrEmpty(request.Model.CorrectOption) &&
                request.Model.CorrectOption.ToLower() != answer.CorrectOption?.ToLower()
                || (request.Model.CorrectNumber.HasValue && request.Model.CorrectNumber != answer?.CorrectNumber)
                || (!string.IsNullOrEmpty(request.Model.CorrectText) && request.Model.CorrectText != answer.CorrectText))
                {
                    studentExercise.WrongAnswers++;
                    throw new WrongAnswerException("Błędna odpowiedź");
                }

                var addedPoints = exercise.Points;

                studentExercise.IsCompleted = true;
                student.CurrentPoints += addedPoints;

                await MediatR.Publish(new PointsAddedEvent(student.Id, addedPoints, student.CurrentPoints), cancellationToken);
                await AchievementService.CheckStudentFinishPath(student.Id,  exercise.LearningPathExercises.First().LearningPathId);
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

        public async Task<Unit> Handle(ExerciseCommand.GameAnswer request, CancellationToken cancellationToken)
        {
            var game = await DbContext.Games.FirstOrDefaultAsync(p => p.PublicId == request.SessionId);

            var exercise = await DbContext.Exercises
                .WithAllIncludes()
                .SingleAsync(p => p.PublicId.ToString() == request.ExercisePublicId);

            var answer = exercise.Answer;

            try
            {
                if (!string.IsNullOrEmpty(request.Model.CorrectOption) &&
                request.Model.CorrectOption.ToLower() != answer.CorrectOption?.ToLower()
                || (request.Model.CorrectNumber.HasValue && request.Model.CorrectNumber != answer?.CorrectNumber)
                || (!string.IsNullOrEmpty(request.Model.CorrectText) && request.Model.CorrectText != answer.CorrectText))            
                    throw new WrongAnswerException("Błędna odpowiedź");

                game.CorrectAnswers++;

            }
            catch (WrongAnswerException)
            {            
                throw;
            }
            finally
            {
               await DbContext.SaveChangesAsync();
            }

            return Unit.Value;
        }
    }

}
