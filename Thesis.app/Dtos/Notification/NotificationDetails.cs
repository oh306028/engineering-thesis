using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Pagination;

namespace Thesis.app.Dtos.Notification
{
    public class NotificationDetails
    {
        public string PublicId { get; set; }

        public string NotifiedBy { get; set; }

        public bool IsSeen { get; set; }

        public string NotificationType { get; set; }

        public bool IsSystemNotification { get; set; }

        public string Message { get; set; }
        public DateTime NotificationDate { get; set; }  
    }

    public class PagedNotificationDetails : PaginationResult<NotificationDetails> { }
}
