using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    //osiagniecia
    //Przykładowo:
    //Pokonaj pierwsze 10 zadań matematycznych na poziomie średnim
    //Pokonaj Learning-path o takiej nazwie
    //Zdobądź pierwsze miejsce w tabeli klasy
    //itd. Każde osiągnięcie będzie miało osobną odznakę
    public class Achievement : IEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }  

        public string Name { get; set; }
        public string Description { get; set; } 

        public Badge Badge { get; set; }
        public int BadgeId { get; set; }

        public List<AchievementStudents> AchievementStudents { get; set; } = new(); 

    }
}
