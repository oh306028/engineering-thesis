using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Answer;

namespace Thesis.app.Dtos.Exercise
{
    public class ExercisePathModel : ExerciseModel
    {
        public AnswerExerciseModel Answer { get; set; }

    }
}
