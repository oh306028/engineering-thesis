using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Student
{
    public class StudentProgressDetails
    {
        public int Level { get; set; }
        public int CurrentPoints { get; set; }

        public int MinLevelPoints { get; set; }
        public int MaxLevelPoints { get; set; }

        public bool NewLevel { get; set; }  


    }
}
