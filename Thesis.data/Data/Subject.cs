using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Subject : IEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }  
        public string Name { get; set; }

        public List<Exercise> Exercises { get; set; } = new List<Exercise>();

    }
}
