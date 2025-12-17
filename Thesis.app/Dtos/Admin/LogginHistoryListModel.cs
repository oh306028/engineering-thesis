using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Pagination;

namespace Thesis.app.Dtos.Admin
{
    public class LogginHistoryListModel
    {
        public DateTime LoginDate { get; set; }
        public bool IsSucceeded { get; set; }
        public string Login { get; set; }           
    }

    public class PagedLogginHistoryListModel : PaginationResult<LogginHistoryListModel> { }
}
