using MediatR;
using Microsoft.EntityFrameworkCore;
using Thesis.app.Dtos.Exercise;
using Thesis.app.Dtos.LearningPath;
using Thesis.app.Exceptions;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Enums;
using Thesis.data.Interfaces;

namespace Thesis.app.Commands
{
    public class LearningPathCommand
    {
        public class CreatePath : IRequest<Unit>
        {
            public LearningPathModel Model { get; set; }
            public int TeacherId { get; set; }
            public CreatePath(LearningPathModel model, int teacherId)
            {
                Model = model;
                TeacherId = teacherId;
            }
        }

        public class CreatePathExercise : IRequest<Unit>
        {
            public Guid Id { get; }
            public ExercisePathModel Model { get; set; }

            public CreatePathExercise(Guid id, ExercisePathModel model)
            {
                Id = id;
                Model = model;
            }
        }

        public class Publish : IRequest<Unit>
        {
            public Guid Id { get; set; }
            public int TeacherId { get; set; }
            public Publish(Guid id, int teacherId)
            {
                Id = id;
                TeacherId = teacherId;
            }
        }

        public class DeleteDraft : IRequest<Unit>
        {
            public Guid Id { get; set; }
            public int TeacherId { get; set; }
            public DeleteDraft(Guid id, int teacherId)
            {
                Id = id;
                TeacherId = teacherId;
            }
        }

    }


    public class LearningPathCommandHandler : IRequestHandler<LearningPathCommand.CreatePath, Unit>,
                                                IRequestHandler<LearningPathCommand.CreatePathExercise, Unit>,
                                                IRequestHandler<LearningPathCommand.Publish, Unit>,
                                                IRequestHandler<LearningPathCommand.DeleteDraft, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public LearningPathCommandHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Unit> Handle(LearningPathCommand.CreatePath request, CancellationToken cancellationToken)
        {
            var learningPath = new LearningPath()
            {
                Type = (int)LearningPathType.Regular,
                IsDraft = true,
                Name = request.Model.Name,
                Level = request.Model.Level,
                CreatedBy = request.TeacherId
            };

            var badge = await DbContext.Badges.FirstOrDefaultAsync(p => p.PublicId == request.Model.BadgeId);
            learningPath.Badges.Add(badge);

            var subject = await DbContext.Subjects.FirstOrDefaultAsync(p => p.PublicId == request.Model.SubjectId);
            learningPath.Subject = subject;

            DbContext.LearningPaths.Add(learningPath);
            await DbContext.SaveChangesAsync();

            return Unit.Value;
        }

        public async Task<Unit> Handle(LearningPathCommand.CreatePathExercise request, CancellationToken cancellationToken)
        {
            var learningPath = await DbContext.LearningPaths
                .Include(p => p.Subject)
                .FirstOrDefaultAsync(p => p.PublicId == request.Id);

            var exercise = new Exercise()
            {
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                CreatedBy = 0,
                Content = request.Model.Content,
                Level = learningPath.Level,
                Subject = learningPath.Subject
            };

            DbContext.Exercises.Add(exercise);

            var answer = new Answer()
            {
                CorrectNumber = request.Model.Answer.CorrectNumber,
                CorrectText = request.Model.Answer.CorrectText,
                CorrectOption = request.Model.Answer.CorrectOption,
                IncorrectOption1 = request.Model.Answer.IncorrectOption1,
                IncorrectOption2 = request.Model.Answer.IncorrectOption2,
                IncorrectOption3 = request.Model.Answer.IncorrectOption3,
                Exercise = exercise
            };

            DbContext.Answers.Add(answer);


            var learningPathExercise = new LearningPathExercises()
            {
                LearningPath = learningPath,
                Exercise = exercise
            };

            DbContext.LearningPathExercises.Add(learningPathExercise);
            await DbContext.SaveChangesAsync();

            return Unit.Value;

        }

        public async Task<Unit> Handle(LearningPathCommand.Publish request, CancellationToken cancellationToken)
        {
            var learningPath = await DbContext.LearningPaths
                .FirstOrDefaultAsync(p => p.PublicId == request.Id);

            if (learningPath.CreatedBy != request.TeacherId)
                throw new Exception("Nie można publikować nie swojej ścieżki");

            learningPath.IsDraft = false;

            await DbContext.SaveChangesAsync();
            return Unit.Value;
        }   

        public async Task<Unit> Handle(LearningPathCommand.DeleteDraft request, CancellationToken cancellationToken)
        {
            var learningPath = await DbContext.LearningPaths
                .Include(p => p.Badges)
                .Include(p => p.LearningPathExercises).ThenInclude(p => p.Exercise)
                .FirstOrDefaultAsync(p => p.PublicId == request.Id);

            if (learningPath.CreatedBy != request.TeacherId)
                throw new Exception("Nie można publikować nie swojej ścieżki");

            if(!learningPath.IsDraft.HasValue || !learningPath.IsDraft.Value)
                 throw new Exception("Nie można usunąć opublikowanej ścieżki");

            DbContext.Remove(learningPath);
            await DbContext.SaveChangesAsync();
            return Unit.Value;

        }
    }
}
