using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Badge : IEntity
    {
        public int Id {get; set; }

        public string Name { get; set; }


        //informacje na temat wygladu takiej odznaki
        //informacje na temat poziomu, za ktory mozna zdobyc odznake
        //informacje na temat osiagniecia, ktore trzeba zrobic by zdobyc odznake


        //sciezka w ktorej mozna zdobyc odznake
        public LearningPath LearningPath { get; set; }
        public int LearningPathId { get; set; }

        //odznaki studenta
        public List<StudentBadges> StudentBadges { get; set; } = new List<StudentBadges>();

    }
}
