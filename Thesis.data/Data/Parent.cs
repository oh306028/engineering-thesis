using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Data
{
    public class Parent : User
    {
        public List<Student> Students { get; set; }

    }
}
