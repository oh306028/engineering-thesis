using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.data.Data
{
    public class User : IEntity, IAuditable, IRemovable
    {
        public int Id { get ; set ; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateDeleted { get; set; }
        public int? DeletedBy { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public List<Notification> RecivedNotifications { get; set; } = new List<Notification>();
        public List<Notification> SentNotifications { get; set; } = new List<Notification>();   

    }
}
