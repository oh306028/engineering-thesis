using Thesis.api.Modules.Answer.Details;
using Thesis.data.Data;

namespace Thesis.api.Modules.Exercise.Details
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
