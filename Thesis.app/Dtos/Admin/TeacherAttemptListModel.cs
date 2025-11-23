using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Admin
{
    public class TeacherAttemptListModel
    {
        public string CertificateUrl { get; set; }
        public bool IsAccepted { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public DateTime DateCreated { get; set; }
        public string PublicId { get; set; }    
    }
}
