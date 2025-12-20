using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Game : IEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }  
        public DateTime DateSessionStarted { get; set; }
        public int? QuestionsCount { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? WrongAnswers => QuestionsCount.Value - CorrectAnswers.Value;

        public int? StudentId { get; set; }
        public Student Student { get; set; }
    }   
}
