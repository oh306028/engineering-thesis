using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Admin
{
    public class UserListModel
    {
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }    
    }
}
