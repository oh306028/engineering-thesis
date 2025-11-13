using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class StudentBadges
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public Badge Badge { get; set; }
        public int BadgeId { get; set; }    
    }
}
