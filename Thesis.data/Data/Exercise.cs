using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Enums;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Exercise : IEntity, IAuditable, IRemovable
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }  
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? DeletedBy { get; set; }
        public string Content { get; set; }     
        public int? Level { get; set; }  
        public ExerciseLevels LevelEnum => (ExerciseLevels)Level;
        public List<LearningPathExercises> LearningPathExercises { get; set; } = new List<LearningPathExercises>(); 
        public List<StudentExercises> StudentExercises { get; set; } = new List<StudentExercises>();
        public Answer Answer { get; set; }

        public int Points => Level * 5 ?? 1 * 5; 
        public int? SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int? HomeWorkId { get; set; }
        public HomeWork HomeWork { get; set; }

    }
}
