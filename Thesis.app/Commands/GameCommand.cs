using MediatR;
using Microsoft.EntityFrameworkCore;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Commands
{
    public class GameCommand
    {
        public class StartSession : IRequest<Guid>
        {
            public int StudentId { get; set; }

            public StartSession(int studentId)
            {
                StudentId = studentId;
            }

        }


        public class GameCommandHandler : IHandler, IRequestHandler<GameCommand.StartSession, Guid>
        {
            public AppDbContext DbContext { get; set; }

            public GameCommandHandler(AppDbContext dbContext)
            {
                DbContext = dbContext;
            }

            public async Task<Guid> Handle(StartSession request, CancellationToken cancellationToken)
            {
                var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(p => p.Id == request.StudentId);

                var game = new Game()
                {
                    DateSessionStarted = DateTime.Now,
                    Student = student,
                    CorrectAnswers = 0,
                    QuestionsCount = 0
                };

                DbContext.Games.Add(game);
                await DbContext.SaveChangesAsync();

                return game.PublicId;

            }
        }

    }
}
