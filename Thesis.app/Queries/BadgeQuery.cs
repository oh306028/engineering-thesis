using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Badge;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Queries
{
    public class BadgeQuery
    {
        public class GetListByPath : IRequest<Badge>
        {
            public string LearningPathId { get; set; }  
            public GetListByPath(string learningPathId)
            {
                LearningPathId = learningPathId;
            }
        }

        public class AreAnyNewRewards : IRequest<bool>
        {

            public int StudentId { get; set; }
            public AreAnyNewRewards(int studentId)
            {
                StudentId = studentId;
            }   
        }

        public class GetAchievements : IRequest<List<Achievement>> 
        {
        
        
        }
        public class GetMineAchievements : IRequest<List<Achievement>> 
        {
            public int StudentId { get; set; }
            public GetMineAchievements(int studentId)
            {
                StudentId = studentId;
            }   
        }


        public class GetListForStudent : IRequest<List<Badge>>
        {
            public int StudentId { get; set; }  
            public GetListForStudent(int studentId)
            {
                StudentId = studentId;
            }
        }
    }


    public class GetListByPathHandler : IRequestHandler<BadgeQuery.GetListByPath, Badge>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public GetListByPathHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<Badge> Handle(BadgeQuery.GetListByPath request, CancellationToken cancellationToken)
        {
            return await DbContext.Badges.Include(p => p.LearningPath).FirstOrDefaultAsync(p => p.LearningPath.PublicId.ToString() == request.LearningPathId);
        }
    }
    public class GetBadgeListForStudentHandler : IRequestHandler<BadgeQuery.GetListForStudent, List<Badge>>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        public GetBadgeListForStudentHandler(AppDbContext dbContext)      
        {
            DbContext = dbContext;
        }

        public async Task<List<Badge>> Handle(BadgeQuery.GetListForStudent request, CancellationToken cancellationToken)
        {
            return await DbContext.Badges.Include(p => p.StudentBadges).Where(p => p.StudentBadges.Any(p => p.StudentId == request.StudentId)).ToListAsync();
        }

    }

    public class GetAchievementList : IRequestHandler<BadgeQuery.GetAchievements, List<Achievement>>, IHandler    
    {
        public AppDbContext DbContext { get; set; }
        public GetAchievementList(AppDbContext dbContext)
        {
            DbContext = dbContext;  
        }
        public async Task<List<Achievement>> Handle(BadgeQuery.GetAchievements request, CancellationToken cancellationToken)
        {
            return await DbContext.Achievements.Include(p => p.Badge).ToListAsync();
        }
    }

    public class GetIfAnyNewRewards : IRequestHandler<BadgeQuery.AreAnyNewRewards, bool>, IHandler  
    {
        public AppDbContext DbContext { get; set; } 
        public GetIfAnyNewRewards(AppDbContext dbContext)
        {
            DbContext = dbContext;  
        }
        public async Task<bool> Handle(BadgeQuery.AreAnyNewRewards request, CancellationToken cancellationToken)
        {
            var newAchievements = await DbContext.AchievementStudents.Where(p => p.StudentId == request.StudentId && !p.IsSeen).ToListAsync();  
            var newBadges = await DbContext.StudentBadges.Where(p => p.StudentId == request.StudentId && !p.IsSeen).ToListAsync();

            return (newAchievements.Any() || newBadges.Any());
        }
    }   

    public class GetMineAchievementList : IRequestHandler<BadgeQuery.GetMineAchievements, List<Achievement>>, IHandler
    {
        public AppDbContext DbContext { get; set; } 
        public GetMineAchievementList(AppDbContext dbContext)   
        {
            DbContext = dbContext;
        }   
        public async Task<List<Achievement>> Handle(BadgeQuery.GetMineAchievements request, CancellationToken cancellationToken)
        {
            return await DbContext.Achievements
                .Include(p => p.Badge)
                .Include(p => p.AchievementStudents)
                .Where(p => p.AchievementStudents.Any(p => p.StudentId == request.StudentId))
                .ToListAsync();
        }
    }

}   
