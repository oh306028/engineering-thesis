using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Data
{
    public class Student : User
    {
        //finished exercises
        public List<StudentExercises> StudentExercises { get; set; } = new List<StudentExercises>();
                
        public Classroom Classroom{ get; set; }
        public int? ClassroomId { get; set; }
        public bool IsAppendingToClass { get; set; }     
        public bool IsAcceptedToClass { get; set; }

        public List<StudentBadges> StudentBadges { get; set; } = new List<StudentBadges>();

        public int ParentId { get; set; }
        public Parent Parent { get; set; }

        public TimeBlocker TimeBlocker { get; set; }

        public int CurrentPoints { get; set; }  
        public AccountLevel AccountLevel { get; set; }
        public int AccountLevelId { get; set; } 
        public List<AchievementStudents> AchievementStudents { get; set; } = new();

        public StudentFilter StudentFilter { get; set; }
        public int StudentFilterId { get; set; }    

        public int CountNewBadges()
        {
            return StudentBadges.Count(sb => !sb.IsSeen);
        }

        public int CountBadges()
        {
            return StudentBadges.Count();
        }

    }
}
