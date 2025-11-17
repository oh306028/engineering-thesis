using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Account;
using Thesis.data;
using Thesis.data.Interfaces;

namespace Thesis.app.Queries
{
    public class AccountQuery
    {
        public class GetRole : IRequest<UserRoleModel>
        {
            public int Id { get; set; } 
            public GetRole(int id)
            {
                Id = id;
            }
        }
    }


    public class GetRoleHandler : IRequestHandler<AccountQuery.GetRole, UserRoleModel>, IHandler
    {
        public AppDbContext DbContext { get; set; }
            

        public GetRoleHandler(AppDbContext dbContext)
        {   
            DbContext = dbContext;
        }
        public async Task<UserRoleModel> Handle(AccountQuery.GetRole request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Users.SingleAsync(p => p.Id == request.Id);

            return new UserRoleModel() { Role = user.Role };
        }
    }
}
