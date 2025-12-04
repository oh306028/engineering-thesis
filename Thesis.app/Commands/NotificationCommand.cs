using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Notification;
using Thesis.app.Queries;
using Thesis.data.Data;
using Thesis.data.Interfaces;
using Thesis.data;
using Microsoft.EntityFrameworkCore;
using Thesis.data.Enums;

namespace Thesis.app.Commands
{
    public class NotificationCommand
    {
        public class SendNotification : IRequest<Unit>
        {
            public NotificationModel Model { get; set; }
            public int UserFrom { get; set; }
            public SendNotification(NotificationModel model, int userFrom)
            {
                Model = model;
                UserFrom = userFrom;
            }
        }
    }

    public class SendNotificationHandler : IRequestHandler<NotificationCommand.SendNotification, Unit>, IHandler
    {
        public AppDbContext DbContext { get; set; }


        public SendNotificationHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<Unit> Handle(NotificationCommand.SendNotification request, CancellationToken cancellationToken)
        {

            var userTo = await DbContext.Users.FirstOrDefaultAsync(p => p.PublicId == Guid.Parse(request.Model.UserToId));
            var message = await DbContext.NotificationMessages.FirstOrDefaultAsync(p => p.PublicId == Guid.Parse(request.Model.MessageId));

            var notification = new Notification()
            {
                MessageId = message.Id,
                UserToId = userTo.Id,
                UserFromId = request.UserFrom,
                DateCreated = DateTime.Now,
                CreatedBy = request.UserFrom,
                IsSystemNotification = false,
                Type = (int)NotificationType.Friendly
            };


            DbContext.Notifications.Add(notification);
            await DbContext.SaveChangesAsync();


            return Unit.Value;
        }
    }
}
