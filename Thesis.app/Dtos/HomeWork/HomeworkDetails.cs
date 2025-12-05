using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Exercise;

namespace Thesis.app.Dtos.HomeWork
{
    public class HomeworkDetails
    {
        public string Type { get; set; }
        public DateTime DeadLine { get; set; }      
        public DateTime DateCreated { get; set; }
        public List<ExerciseHomeWorkDetails> Exercises { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string PublicId { get; set; }    
    }
}
