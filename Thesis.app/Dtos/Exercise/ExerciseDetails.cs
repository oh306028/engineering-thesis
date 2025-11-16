using Thesis.app.Dtos.Answer;
using Thesis.data.Data;

namespace Thesis.app.Dtos.Exercise
{
    public class ExerciseDetails
    {
        public string PublicId { get; set; }

        public string Level { get; set; }
        public string LearningPath { get; set; }    
        public bool IsDone { get; set; }

        public AnswerDetails Answer { get; set; }

        public string Subject { get; set; } 
    }
}
