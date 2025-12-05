using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Enums
{
    public enum HomeWorkType
    {
        [Description("Powtórkowe")]
        Review,
        [Description("Zwykłe")]
        Casual
    }
}
