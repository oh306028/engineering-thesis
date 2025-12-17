using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Pagination;

namespace Thesis.app.Dtos.Admin
{
    public class ClassroomListModel
    {
        public DateTime DateCreated { get; set; }
        public string ClassName { get; set; }
        public string ClassroomKey { get; set; }
        public string TeacherName { get; set; } 
    }

    public class PagedClassroomListModel : PaginationResult<ClassroomListModel> { }
}
