using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Enums;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{

    public class HomeWork : IEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }  

        public int Type { get; set; }
        public HomeWorkType TypeEnum => (HomeWorkType)Type;

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        public List<Exercise> Exercises { get; set; } = new List<Exercise>();

        public DateTime DateCreated { get; set; }   
        public DateTime DateModified { get; set; }
        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; } 

    }
}
