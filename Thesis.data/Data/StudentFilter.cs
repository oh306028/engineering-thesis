using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class StudentFilter : IEntity
    {
        public int Id { get; set; }

        public DateTime DateSet { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }        
        public int Level { get; set; }
        public Student Student { get; set; }

    }
}
