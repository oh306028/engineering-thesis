using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.LearningPath;

namespace Thesis.app.Dtos.Resource
{
    public class PathExercisesResource
    {
        public int Count { get; set; }
        public List<PathExercise> Exercises { get; set; } = new List<PathExercise>();

        public PathExercisesResource(List<PathExercise> exercises)
        {
            Exercises = exercises;
            Count = exercises.Count;
        }
    }
}
