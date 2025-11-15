using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Classroom : IEntity, IAuditable
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }  
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }

        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
      
        public string ClassName { get; set; }

        public string ClassroomKey { get; set; }    

        public List<Student> Students { get; set; } = new List<Student>();

        public List<HomeWork> HomeWorks { get; set; } = new List<HomeWork>();

    }
}
