using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.api.Extensions;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;


namespace Thesis.app.Queries
{
    public class LearningPathQuery
    {
        public class GetList : IRequest<List<LearningPath>>
        {
            public GetList()
            {
                
            }
        }

        public class GetExercises : IRequest<List<Exercise>>
        {
            public string PublicId { get; set; }
            public GetExercises(string publicId)
            {
                PublicId = publicId;
            }
        }
    }


    public class GetLearningPathListHandler : IRequestHandler<LearningPathQuery.GetList, List<LearningPath>>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public GetLearningPathListHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }    

        public async Task<List<LearningPath>> Handle(LearningPathQuery.GetList request, CancellationToken cancellationToken)
        { 
            return await DbContext.LearningPaths.AsNoTracking().ToListAsync(cancellationToken);
        }
    }

    public class GetExercisesPerPathHandler : IRequestHandler<LearningPathQuery.GetExercises, List<Exercise>>, IHandler
    {
        public AppDbContext DbContext { get; set; }     
        public GetExercisesPerPathHandler(AppDbContext dbContext)   
        {
            DbContext = dbContext;
        }

        public async Task<List<Exercise>> Handle(LearningPathQuery.GetExercises request, CancellationToken cancellationToken)
        {
            return await DbContext.Exercises
                .WithAllIncludes()
                .AsNoTracking()
                .Where(p => p.LearningPathExercises.Any(p => p.LearningPath.PublicId.ToString() == request.PublicId))
                .ToListAsync();
        }
    }
}
