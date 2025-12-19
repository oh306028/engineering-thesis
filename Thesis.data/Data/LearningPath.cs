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

        public int Level { get; set; }

        public int? CreatedBy { get; set; }
        public Teacher Teacher { get; set; }        

        public string Name { get; set; }    

        public int Type { get; set; }
        public LearningPathType EnumType => (LearningPathType)Type;

        public List<Badge> Badges { get; set; } = new();

        public bool? IsDraft { get; set; }

        public Subject Subject { get; set; }
        public int? SubjectId { get; set; }  


        //moze jednak trzeba tabele studentLearningPath dodac? aby zaznaczac ze student ukonczyl juz jakas learningPath?
        //chyba ze sprawdzac to po studentExercises? mam taka logike w nagrodach za zakonczenie path, cos podobnego trzeba zrobic
        //istnieje mozliwosc ze bedziemy zwracac wszystkie mozliwe sciezki ale na froncie na podstawie isFinished nie pokazemy ich, a damy mozliwosc odswiezenia sciezek?
        //czyli wylosowania 3 innych? jesli przykladowo bedzie wiecej niz 3 sciezki? jest to jakies proste rozwiazanie


        //zeby pozniej zaimplementowac filtrowanie sciezek dla ucznia przez rodzica, nalezaloby jakos utrzymywac informacje o uczniu, jakie dzisiaj ma ustawione filtry
        //nastepnie wystarczyloby wysylac te filtry jako query params dla geta dla tych sciezek wiec nie musimy miec nic wiecej
        //=> mamy juz tabele, bedziemy robic PUT rodzicem na te filtry, ustawiajac przedmiot sciezki ktore maja sie widzisaj wysweitlac
    }
}
