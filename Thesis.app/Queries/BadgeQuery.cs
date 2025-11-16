using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Badge;
using Thesis.data.Data;

namespace Thesis.app.Queries
{
    public class BadgeQuery
    {
        public class GetListByPath : IRequest<List<Badge>>
        {
            public GetListByPath()
            {
                
            }
        }

        public class GetListForStudent : IRequest<List<Badge>>
        {
            public GetListForStudent()
            {
                
            }
        }
    }


    public class GetListByPathHandler : IRequestHandler<BadgeQuery.GetListByPath, List<Badge>>
    {
        public GetListByPathHandler()
        {
            
        }
        public Task<List<Badge>> Handle(BadgeQuery.GetListByPath request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
    public class GetBadgeListForStudentHandler : IRequestHandler<BadgeQuery.GetListForStudent, List<Badge>>
    {
        public GetBadgeListForStudentHandler()      
        {
            
        }
        public Task<List<Badge>> Handle(BadgeQuery.GetListForStudent request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}   
