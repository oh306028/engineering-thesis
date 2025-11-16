using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Dtos.Notification
{
    public class NotificationList
    {
        public string PublicId { get; set; }

        public string NotifiedBy { get; set; }

        public bool IsSeen { get; set; }

        public string NotificationType { get; set; }    
    }
}
