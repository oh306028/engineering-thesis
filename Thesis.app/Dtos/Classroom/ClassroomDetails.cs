using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Student;

namespace Thesis.app.Dtos.Classroom
{
    public class ClassroomDetails
    {
        public string ClassName { get; set; }   
        public string ClassroomKey { get; set; }    
        public string TeacherName { get; set; }
        public string PublicId { get; set; }

        public string TeacherPublicId { get; set; } 

    }
}
