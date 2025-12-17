using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Enums;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class LearningPath : IEntity 
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }
        public List<LearningPathExercises> LearningPathExercises { get; set; } = new List<LearningPathExercises>(); 

        public int Level { get; set; }  //1,2,3

        public string Name { get; set; }    

        public int Type { get; set; }
        public LearningPathType EnumType => (LearningPathType)Type;

        public List<Badge> Badges { get; set; } //odznaki ktore mozna zdobyc za ukonczenie levela


        //tworzenie sciezek przez nauczycieli
        //dodanie subjectu? aby pozniej rodzic mogl filtrowac wlasnie po przedmiocie i poziomie trudnosci?
        public bool? IsDraft { get; set; }   

        //moze jednak trzeba tabele studentLearningPath dodac? aby zaznaczac ze student ukonczyl juz jakas learningPath?
        //chyba ze sprawdzac to po studentExercises? mam taka logike w nagrodach za zakonczenie path, cos podobnego trzeba zrobic
    }
}
