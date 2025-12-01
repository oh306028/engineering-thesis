using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Enums;

namespace Thesis.app.Dtos.Notification
{
    public class NotificationModel
    {
        public string UserToId { get; set; }
        public string Message { get; set; }
        public NotificationType  NotificationType { get; set; } 

    }
}
