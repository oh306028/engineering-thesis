using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Student
{
    public class StudentDetailsWithClassroom : StudentDetails
    {
        public string TeacherPublicId { get; set; }
        public string TeacherName { get; set; }

        public string FilterSubject { get; set; }
        public int? FilterLevel { get; set; }   
    }   
}
