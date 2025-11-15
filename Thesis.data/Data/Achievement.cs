using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Achievement : IEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }  

        public string Name { get; set; }    

        public Badge Badge { get; set; }
        public int BadgeId { get; set; }

        public Student AchievedBy { get; set; } 
        public int AchievedById { get; set; }   


    }
}
