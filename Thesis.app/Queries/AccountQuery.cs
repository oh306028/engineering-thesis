using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Account;
using Thesis.data;
using Thesis.data.Data;
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

        public class GetProfileDetails : IRequest<User>
        {

            public int UserId { get; set; }
            public GetProfileDetails(int userId)
            {
                UserId = userId;
            }

        }

    }


    public class GetRoleHandler : IRequestHandler<AccountQuery.GetRole, UserRoleModel>,
        IRequestHandler<AccountQuery.GetProfileDetails, User>,IHandler
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
            
        public async Task<User> Handle(AccountQuery.GetProfileDetails request, CancellationToken cancellationToken)
        {
            return await DbContext.Users.SingleAsync(p => p.Id == request.UserId);
        }
    }
}
