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
            public int UserId { get; set; }
            public GetList(LearningPathType type, int userId)
            {
                Type = type;
                UserId = userId;
            }
        }

        public class GetDraftsList : IRequest<List<LearningPath>>
        {
            public int TeacherId { get; set; }
            public GetDraftsList(int teacherId)
            {
                TeacherId = teacherId;
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


    public class GetLearningPathListHandler : IHandler,
        IRequestHandler<LearningPathQuery.GetList, List<LearningPath>>,
        IRequestHandler<LearningPathQuery.GetDraftsList, List<LearningPath>>
    {
        public AppDbContext DbContext { get; set; }
        public GetLearningPathListHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }    

        public async Task<List<LearningPath>> Handle(LearningPathQuery.GetList request, CancellationToken cancellationToken)
        {
            var student = await DbContext.Users.OfType<Student>()
                .Include(p => p.StudentFilter)
                .FirstOrDefaultAsync(p => p.Id == request.UserId);

            var filters = student.StudentFilter;
            var subject = await DbContext.Subjects.FirstOrDefaultAsync(p => p.Id == filters.SubjectId);

            return await DbContext.LearningPaths
                .Include(p => p.Badges)
                .Include(p => p.Subject)
                .AsNoTracking()
                .Where(p => p.Type == (int)request.Type && (!p.IsDraft.HasValue || !p.IsDraft.Value) && p.Subject == subject && p.Level == filters.Level)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<LearningPath>> Handle(LearningPathQuery.GetDraftsList request, CancellationToken cancellationToken)
        {
            return await DbContext.LearningPaths
               .Include(p => p.Badges)
               .Include(p => p.Subject) 
               .AsNoTracking()
               .Where(p => p.CreatedBy == request.TeacherId && p.IsDraft.HasValue && p.IsDraft.Value)
               .ToListAsync(cancellationToken);
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
