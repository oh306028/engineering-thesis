using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Answer : IEntity
    {
        public int Id { get; set; }

        public string CorrectText { get; set; }
        public int? CorrectNumber { get; set; }
        public int? CorrectOption { get; set; }
        public int ExerciseId { get; set; } 
        public Exercise Exercise { get; set; }

    }
}
