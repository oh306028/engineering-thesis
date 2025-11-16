using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Queries
{
    public class NotificationQuery
    {
        public class GetUserNotifications : IRequest<List<Notification>> 
        {
            public int UserId { get; set; }
            public GetUserNotifications(int userId)
            {
                UserId = userId;
            }

        }

        public class GetNotificatioDetails : IRequest<Notification>
        {
            public int UserId { get; set; }
            public string PublicId { get; set; }    
            public GetNotificatioDetails(int userId, string publicId)
            {
                UserId = userId;
                PublicId = publicId;
            }

        }

    }   

    public class GetUserNotificationsHandler : IRequestHandler<NotificationQuery.GetUserNotifications, List<Notification>>, IHandler
    {
        public AppDbContext DbContext { get; set; }


        public GetUserNotificationsHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<List<Notification>> Handle(NotificationQuery.GetUserNotifications request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Users
                .Include(p => p.RecivedNotifications).ThenInclude(p => p.Message)
                .AsNoTracking()
                .SingleAsync(p => p.Id == request.UserId);

            return user.RecivedNotifications;

        }
    }
}
