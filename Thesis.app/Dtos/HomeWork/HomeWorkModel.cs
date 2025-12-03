using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Exercise;
using Thesis.data.Enums;

namespace Thesis.app.Dtos.HomeWork
{
    public class HomeWorkModel
    {
        public HomeWorkType Type { get; set; }
        public List<ExerciseModel> Exercises{ get; set; }
        public DateTime DeadLine { get; set; }  
    }
}
