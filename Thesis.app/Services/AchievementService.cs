using Microsoft.EntityFrameworkCore;
using Thesis.data;
using Thesis.data.Data;

namespace Thesis.app.Services
{
    public interface IAchievementService
    {
        Task CheckStudentExercisesAwards(int studentId);
        Task CheckStudentFinishPath(int studentId, int pathId);
        Task CheckStudentLevelAwards(int studentId);
        Task CheckStudentLogginAwards(int studentId, DateTime logInDate);
    }

    public class AchievementService : IAchievementService
    {
        public AppDbContext DbContext { get; }

        public AchievementService(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task CheckStudentLogginAwards(int studentId, DateTime logInDate)
        {

        }

        public async Task CheckStudentLevelAwards(int studentId)
        {

        }

        public async Task CheckStudentFinishPath(int studentId, int pathId)
        {
            var requiredExerciseIds = await DbContext.LearningPathExercises
                .Where(lpe => lpe.LearningPathId == pathId)
                .Select(lpe => lpe.ExerciseId)
                .ToListAsync();

         
            if (!requiredExerciseIds.Any())                     
                return;
            

            var completedCount = await DbContext.StudentExercises
                .Where(se => se.StudentId == studentId)
                .Where(se => se.IsCompleted)
                .CountAsync(se => requiredExerciseIds.Contains(se.ExerciseId));

            //wywolujemy serwis, gdy zadanie jest dopiero wykonywane poprawnie
            if (completedCount == requiredExerciseIds.Count - 1)
            {
                var path = await DbContext.LearningPaths.Include(p => p.Badges).FirstOrDefaultAsync(p => p.Id == pathId);

                var badge = path?.Badges.FirstOrDefault();

                if (badge != null)
                {
                    var alreadyHasBadge = await DbContext.StudentBadges
                        .AnyAsync(sb => sb.StudentId == studentId && sb.BadgeId == badge.Id);

                    if (!alreadyHasBadge)
                    {
                        var student = await DbContext.Users.OfType<Student>().FirstOrDefaultAsync(p => p.Id == studentId);
                        if (student.StudentBadges == null)
                        {
                            student.StudentBadges = new List<StudentBadges>();
                        }

                        student.StudentBadges.Add(new StudentBadges() { Badge = badge, Student = student, IsSeen = false });

                        await DbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task CheckStudentExercisesAwards(int studentId)
        {

        }

        //private bool HasLogged14DaysARow()
        //{

        //}

        //private bool Got100Points()
        //{

        //}


        //private bool Got1000Exercises()
        //{

        //}

        //private bool Got1000Exercises()
        //{

        //}

        //private bool FinishedNatureHistoryPath()
        //{

        //}

        //private bool Done100WordExercises()
        //{

        //}

        //private bool Finished30MathExercises()
        //{

        //}

        //private bool Finished3PathsFromRegularPathType()
        //{

        //}

        //private bool Finished20LogicExercises()
        //{

        //}



    }
}
