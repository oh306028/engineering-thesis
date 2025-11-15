using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class TimeBlocker : IEntity
    {
        public int Id { get; set; }

        public Guid PublicId { get; set; }  

        public Student Student { get; set; }
        public int StudentId { get; set; }

        public TimeSpan? MaxTimePerDay  { get; set; }
        public TimeSpan? MaxTimePerSession  { get; set; }    

        public bool IsBlocked { get; set; }

        public DateTime? BLockStartDateSession { get; set; }
        public DateTime? BlockEndDateSession { get; set; }

        public DateTime? BlockDayDate { get; set; } 

    }
}
