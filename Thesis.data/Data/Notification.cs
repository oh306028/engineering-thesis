using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Enums;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class Notification : IEntity, IAuditable
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }

        public bool IsSystemNotification { get; set; }  

        public int? UserFromId { get; set; }
        public int UserToId { get; set; }

        public User UserFrom { get; set; }
        public User UserTo { get; set; }    

        public bool IsSeen { get; set; }
        public NotificationMessage Message { get; set; }
        public int MessageId { get; set; }

        public int Type { get; set; }   
        public NotificationType TypeEnum => (NotificationType)Type;





    }
}
