using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.data.Interfaces
{
    public interface IRemovable
    {
        public DateTime? DateDeleted { get; set; }
        public int? DeletedBy { get; set; }  
    }
}
