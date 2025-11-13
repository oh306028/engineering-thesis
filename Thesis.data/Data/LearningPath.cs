using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Enums;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class LearningPath : IEntity 
    {
        public int Id { get; set; }
        public List<LearningPathExercises> LearningPathExercises { get; set; } = new List<LearningPathExercises>(); 

        public int Level { get; set; }  //1,2,3

        public int Type { get; set; }
        public LearningPathType EnumType => (LearningPathType)Type;

        public List<Badge> Badges { get; set; } //odznaki ktore mozna zdobyc za ukonczenie levela

    }
}
