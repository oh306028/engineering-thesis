using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Admin
{
    public class LogginHistoryListModel
    {
        public DateTime LoginDate { get; set; }
        public bool IsSucceeded { get; set; }
        public string UserEmail { get; set; }         
    }
}
