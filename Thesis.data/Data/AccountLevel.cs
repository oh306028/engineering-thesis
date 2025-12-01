using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Data
{
    public class AccountLevel
    {
        public int Id { get; set; } 

        public int Level { get; set; }
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }

        public List<Student> Students { get; set; } = new();
    }
}
