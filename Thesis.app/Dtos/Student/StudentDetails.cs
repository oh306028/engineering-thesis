using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Student
{
    public class StudentDetails
    {
        public string PublicId { get; set; }    
        public string Name { get; set; }

        public int Level { get; set; }
        public int CurrentPoints { get; set; }
        public int BadgesCount { get; set; }

        public DateTime LastSeenAt { get; set; }
        public bool IsCurrentUser { get; set; }

    }
}
