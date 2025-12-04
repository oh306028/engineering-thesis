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

namespace Thesis.app.EventHandlers
{
    public class LevelUpHandler : INotificationHandler<PointsAddedEvent>
    {
        public AppDbContext DbContext { get; set; }
        public LevelUpHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Handle(PointsAddedEvent notification, CancellationToken cancellationToken)
        {
            var student = DbContext.Users.OfType<Student>().First(p => p.Id == notification.StudentId);
            var accountLevels = DbContext.AccountLevels.ToList();
            var currentStudentLevel = student.AccountLevel; 

            if(student.CurrentPoints > currentStudentLevel.MaxPoints && student.AccountLevel.Level != 7)
            {
                student.AccountLevel = accountLevels.First(p => p.Level == currentStudentLevel.Level + 1);
                await DbContext.SaveChangesAsync();
            }

        }
    }
}
