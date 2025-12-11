using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class NotificationMessage : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<Notification> Notifications { get; set; } = new();
        public int NotificationId { get; set; }

        public Guid PublicId { get; set; }      


    }
}
