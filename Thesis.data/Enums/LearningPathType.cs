using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Enums
{
    public enum LearningPathType
    {
        [Description("Powtórka")]
        Review,
        [Description("Nauka")]
        Regular,
        [Description("Wyzwanie")]
        Challenge,
    }
}
