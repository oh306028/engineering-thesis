using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class StudentExercises
    {
        public Exercise Exercise { get; set; }
        public int ExerciseId { get; set; }
        public Student Student { get; set; }
        public int StudentId { get; set; }  
    }
}
