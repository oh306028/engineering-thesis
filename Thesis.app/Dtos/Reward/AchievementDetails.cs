using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Badge
{
    public class AchievementDetails
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BadgeDetails Badge { get; set; }
    }
}
