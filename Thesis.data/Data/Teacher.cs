using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Data
{
    public class Teacher : User
    {
        public List<Classroom> Classrooms { get; set; } = new();
        public List<HomeWork> HomeWorks { get; set; } = new();
        public List<LearningPath> LearningPaths { get; set; } = new();
            
        public string CertificateUrl { get; set; }
        public bool? IsAccepted { get; set; } 

    }   
}
