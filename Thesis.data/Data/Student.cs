using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Data
{
    public class Student : User
    {
        //finished exercises
        public List<StudentExercises> StudentExercises { get; set; } = new List<StudentExercises>();
                
        //classroom stuff
        public Classroom Classroom{ get; set; }
        public int? ClassroomId { get; set; }
        public bool IsAppendingToClass { get; set; }     
        public bool IsAcceptedToClass { get; set; }

        //

        public List<StudentBadges> StudentBadges { get; set; } = new List<StudentBadges>();


        //konto ucznia powiazane z rodzicem!
        public int ParentId { get; set; }
        public Parent Parent { get; set; }




    }
}
