using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Classroom;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;


namespace Thesis.app.Queries
{
    public class ClassroomQuery
    {
        public class GetDetails : IRequest<Classroom> 
        {
            public string PublicId { get; set; }
            public GetDetails(string publicId)
            {
                PublicId = publicId;
            }

        }   

        public class GetList : IRequest<List<Classroom>>
        {
            public int TeacherId { get; set; }
            public GetList(int teacherId)
            {
                TeacherId = teacherId;
            }

        }

    }

    public class GetClassroomDetailsHandler : IRequestHandler<ClassroomQuery.GetDetails, Classroom>, IHandler
    {
        public AppDbContext DbContext { get; set; } 
        public GetClassroomDetailsHandler(AppDbContext dbContext)
        {   
            this.DbContext = dbContext;
        }   

        public async Task<Classroom> Handle(ClassroomQuery.GetDetails request, CancellationToken cancellationToken)
        {
            return await DbContext.Classrooms.Include(p => p.Students).FirstOrDefaultAsync(p => p.PublicId.ToString() == request.PublicId);
        }   
    }

    public class GetClassroomListHandler : IRequestHandler<ClassroomQuery.GetList, List<Classroom>>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public GetClassroomListHandler(AppDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<List<Classroom>> Handle(ClassroomQuery.GetList request, CancellationToken cancellationToken)
        {
            return await DbContext.Classrooms.Where(p => p.TeacherId == request.TeacherId).ToListAsync();
        }
    }

}
