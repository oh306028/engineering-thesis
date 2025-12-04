using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Events;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Enums;

namespace Thesis.app.EventHandlers
{
    public class CreatedHomeWorkEventHandler : INotificationHandler<CreatedHomeWorkEvent>
    {
        public AppDbContext DbContext { get; }

        public CreatedHomeWorkEventHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Handle(CreatedHomeWorkEvent notification, CancellationToken cancellationToken)
        {
            var classroom = DbContext.Classrooms
                .Include(p => p.Students)
                .FirstOrDefault(p => p.Id == notification.ClassroomId);

            var notifications = new List<Notification>();

            foreach (var student in classroom.Students)
            {
                notifications.Add(new Notification()
                {
                    DateCreated = DateTime.Now,
                    UserToId = student.Id,
                    Type = (int)NotificationType.Info,
                    IsSystemNotification = true,
                    CreatedBy = 0,
                    Message = DbContext.NotificationMessages.FirstOrDefault(p => p.Name == "Nowe zadanie domowe!")
                });

            }

            await DbContext.AddRangeAsync(notifications);
            await DbContext.SaveChangesAsync();

        }
    }

}
