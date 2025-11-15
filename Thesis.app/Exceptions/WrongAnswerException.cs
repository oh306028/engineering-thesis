using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Exceptions
{
    public class WrongAnswerException : Exception
    {
        public WrongAnswerException(string message) : base(message)
        {
            
        }
    }
}
