using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class LearningPathExercises
    {
        public int LearningPathId { get; set; }
        public LearningPath LearningPath { get; set; }

        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; }  
    }
}
