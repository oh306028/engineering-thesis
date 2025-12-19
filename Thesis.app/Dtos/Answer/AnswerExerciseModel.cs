using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Answer
{
    public class AnswerExerciseModel : AnswerModel
    {
        public string IncorrectOption1 { get; set; }
        public string IncorrectOption2 { get; set; }
        public string IncorrectOption3 { get; set; }
    }
}
