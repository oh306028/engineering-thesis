using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Events;
using Thesis.app.Services;
using Thesis.data;

namespace Thesis.app.EventHandlers
{
    public class LogInHandler : INotificationHandler<LogInEvent>
    {
        public AppDbContext DbContext { get; }
        public IAchievementService AchievementService { get; }

        public LogInHandler(AppDbContext dbContext, IAchievementService achievementService)
        {
            DbContext = dbContext;
            AchievementService = achievementService;
        }
        public async Task Handle(LogInEvent notification, CancellationToken cancellationToken)
        {
            await AchievementService.CheckStudentLogginAwards(notification.StudentId, notification.LogInDate);
        }
    }
}
