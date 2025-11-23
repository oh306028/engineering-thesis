using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Commands
{
    public class AdminCommand 
    {
        public class ManageTeacherAttempt : IRequest<Unit>
        {
            public bool IsAccepted { get; set; }
            public string PublicId { get; set; }    
            public ManageTeacherAttempt(string publicId, bool isAccepted)
            {
                PublicId = publicId;
                IsAccepted = isAccepted;
            }
        }   
    }



    public class ManageTeacherAttemptHandler : IRequestHandler<AdminCommand.ManageTeacherAttempt, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public ManageTeacherAttemptHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<Unit> Handle(AdminCommand.ManageTeacherAttempt request, CancellationToken cancellationToken)
        {
            var teacher = DbContext.Users.OfType<Teacher>().Single(p => p.PublicId.ToString() == request.PublicId);

            if (request.IsAccepted)          
                teacher.IsAccepted = true;
            else        
                DbContext.Users.Remove(teacher);

            await DbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
 