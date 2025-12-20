using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.api.Extensions;
using Thesis.app.Exceptions;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Queries
{
    public class GameQuery
    {
        public class GetQuestion : IRequest<Exercise>
        {
            public Guid SessionId { get; set; }
            public GetQuestion(Guid sessionId)
            {
                SessionId = sessionId;
            }
        }

        public class GetDetails : IRequest<Game>
        {
            public Guid SessionId { get; set; }
            public GetDetails(Guid sessionId)
            {
                SessionId = sessionId;
            }
        }   

    }


    public class GameQueryHandler : IHandler,
                                IRequestHandler<GameQuery.GetQuestion, Exercise>,
                                IRequestHandler<GameQuery.GetDetails, Game>         
    {
        public AppDbContext DbContext { get; set; }

        public GameQueryHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Exercise> Handle(GameQuery.GetQuestion request, CancellationToken cancellationToken)
        {

            var game = await DbContext.Games.FirstOrDefaultAsync(p => p.PublicId == request.SessionId);

            if ((DateTime.Now - game.DateSessionStarted) >= TimeSpan.FromMinutes(1))
                throw new NotFoundException("Koniec czasu");

            game.QuestionsCount++;

           var exercises = await DbContext.Exercises
                .WithAllIncludes()
                .ToListAsync();

            var draftPaths = await DbContext.LearningPaths
                .Include(p => p.LearningPathExercises)
                .Where(p => p.IsDraft.HasValue && p.IsDraft.Value)
                .Select(p => p.LearningPathExercises)
                .ToListAsync();

            var currentExercises = exercises.Where(p => !draftPaths.Contains(p.LearningPathExercises)).ToList();
            var random = new Random();

            DbContext.SaveChangesAsync();

            return currentExercises[random.Next(0, currentExercises.Count + 1)];
        }

        public async Task<Game> Handle(GameQuery.GetDetails request, CancellationToken cancellationToken)
        {
            return await DbContext.Games.FirstOrDefaultAsync(p => p.PublicId == request.SessionId);
        }
    }
}
