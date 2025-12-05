using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.api.Extensions;
using Thesis.app.Services;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Enums;
using Thesis.data.Interfaces;


namespace Thesis.app.Queries
{
    public class LearningPathQuery
    {
        public class GetList : IRequest<List<LearningPath>>
        {
            public LearningPathType Type { get; set; }
            public GetList(LearningPathType type)
            {
                Type = type;
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

        public class GetHardestExercises : IRequest<List<Exercise>>
        {
            public int StudentId { get; set; }
            public GetHardestExercises(int studentId)
            {
                StudentId = studentId;
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
            return await DbContext.LearningPaths.Include(p => p.Badges).AsNoTracking().Where(p => p.Type == (int)request.Type).ToListAsync(cancellationToken);
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

    public class GetHardestExercisesHandler : IRequestHandler<LearningPathQuery.GetHardestExercises, List<Exercise>>
    {
        public IAdaptiveSystemService AdaptiveSystem { get; }
        public GetHardestExercisesHandler(IAdaptiveSystemService adaptiveSystem)
        {
            AdaptiveSystem = adaptiveSystem;
        }

        public async Task<List<Exercise>> Handle(LearningPathQuery.GetHardestExercises request, CancellationToken cancellationToken)
        {
            return await AdaptiveSystem.GetHardestExercises(request.StudentId);
        }
    }
}
