using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Enums
{
    public enum ExerciseLevels
    {
        [Description("Łatwy")]
        Easy,
        [Description("Średni")]
        Medium,
        [Description("Trudny")]
        Hard,
        [Description("Bardzo trudny")]
        VeryHard,
        [Description("Arcy trudny")]
        Hardest

    }
}
