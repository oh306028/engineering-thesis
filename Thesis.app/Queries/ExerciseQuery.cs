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
    public class ExerciseQuery
    {

        public class GetList : IRequest<List<Exercise>>
        {
            public GetList()
            {
                
            }

        }

        public class GetListByStudentId : IRequest<List<Exercise>>
        {
            public string PublicId { get; set; }    
            public GetListByStudentId(string publicId)
            {   
                PublicId = publicId;
            }
        }

        public class GetDetails : IRequest<Exercise> 
        {
            public string PublicId { get; set; }    
            public GetDetails(string publicId)
            {
                PublicId = publicId;
            }
        }

        public class GetAnswer : IRequest<Answer> 
        {
            public string PublicId { get; set; }    
            public GetAnswer(string publicId)
            {
                PublicId = publicId;
            }

        }


    }

    public class GetAnswerHandler : IRequestHandler<ExerciseQuery.GetAnswer, Answer>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public GetAnswerHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Answer> Handle(ExerciseQuery.GetAnswer request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Answers
                .Include(p => p.Exercise)
                .FirstOrDefaultAsync(p => p.Exercise.PublicId.ToString() == request.PublicId);

            if (result == null)
                throw new NotFoundException("Exercise not found");

            return result;
        }
    }


    public class GetDetailsHandler : IRequestHandler<ExerciseQuery.GetDetails, Exercise>, IHandler
    {
        public AppDbContext DbContext { get; set; }

        public GetDetailsHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Exercise> Handle(ExerciseQuery.GetDetails request, CancellationToken cancellationToken)
        {
            var result =  await DbContext.Exercises
               .WithAllIncludes()
               .AsNoTracking()
               .FirstOrDefaultAsync(p => p.PublicId.ToString() == request.PublicId);

            if (result == null)
                throw new NotFoundException("Exercise not found");

            return result;
        }
    }

    public class GetListHandler : IRequestHandler<ExerciseQuery.GetList, List<Exercise>>, IHandler
    {
        public AppDbContext DbContext { get; set; } 
        public GetListHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<List<Exercise>> Handle(ExerciseQuery.GetList request, CancellationToken cancellationToken)
        {
            return await DbContext.Exercises
                .WithAllIncludes()
                .AsNoTracking()      
                .ToListAsync();
        }
    }

    public class GetListForStudentHandler : IRequestHandler<ExerciseQuery.GetListByStudentId, List<Exercise>>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public GetListForStudentHandler(AppDbContext dbContext)
        {   
            DbContext = dbContext;
        }
        public async Task<List<Exercise>> Handle(ExerciseQuery.GetListByStudentId request, CancellationToken cancellationToken)
        {
            var student = await DbContext.Users
                .FirstOrDefaultAsync(p => p.PublicId.ToString() == request.PublicId);

            if (student == null)
                throw new NotFoundException("Student not found");

            return await DbContext.Exercises
                .WithAllIncludes()
                .Where(e => e.StudentExercises.Any(se => se.StudentId == student.Id))
                .ToListAsync();
        }


    }


}
