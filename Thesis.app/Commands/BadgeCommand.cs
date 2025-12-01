using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data;
using Thesis.data.Interfaces;

namespace Thesis.app.Commands
{
    public class BadgeCommand
    {
        public class MarkAsSeen : IRequest<Unit>
        {
            public int StudentId { get; set; }
            public List<string> RewardIds { get; set; }

            public MarkAsSeen(int studentId, List<string> rewardIds)    
            {
                StudentId = studentId;
                RewardIds = rewardIds;
            }
        }

        public class MarkAsSeenHandler : IRequestHandler<BadgeCommand.MarkAsSeen, Unit>, IHandler
        {
            public AppDbContext DbContext { get; set; }

            public MarkAsSeenHandler(AppDbContext dbContext)
            {
                DbContext = dbContext;
            }

            public async Task<Unit> Handle(MarkAsSeen request, CancellationToken cancellationToken)
            {
                var rewardGuids = request.RewardIds
                    .Select(Guid.Parse)
                    .ToList();

                var achievements = await DbContext.AchievementStudents
                    .Include(p => p.Achievement)
                    .Where(p =>
                        p.StudentId == request.StudentId &&
                        rewardGuids.Contains(p.Achievement.PublicId))
                    .ToListAsync(cancellationToken);

                foreach (var a in achievements)
                    a.IsSeen = true;

                var badges = await DbContext.StudentBadges
                    .Include(p => p.Badge)
                    .Where(p =>
                        p.StudentId == request.StudentId &&
                        rewardGuids.Contains(p.Badge.PublicId))
                    .ToListAsync(cancellationToken);

                foreach (var b in badges)
                    b.IsSeen = true;

                await DbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

        }

    }
}
