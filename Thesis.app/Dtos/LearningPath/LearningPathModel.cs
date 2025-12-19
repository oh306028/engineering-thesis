using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Enums;

namespace Thesis.app.Dtos.LearningPath
{
    public class LearningPathModel
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public Guid SubjectId { get; set; }
        public Guid BadgeId { get; set; }             
    }
}
