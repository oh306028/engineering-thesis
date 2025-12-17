using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Filters.NotificationQueryFilters
{
    public class NotificationListFilter : IFIlter<Notification>
    {
        public bool? IsSeen { get; set; } = false;
        public string NotifiedBy { get; set; }
        public DateTime? NotificationDate { get; set; }
        public Expression<Func<Notification, bool>> GetPredicate()
        {
            var predicate = PredicateBuilder.New<Notification>(true);

            if (NotificationDate.HasValue)
            {
                Expression<Func<Notification, bool>> dateFromFilter =
                    entry => entry.DateCreated >= NotificationDate.Value;

                predicate = predicate.And(dateFromFilter);
            }

            if (IsSeen.HasValue)
            {
                predicate = predicate.And(entry => entry.IsSeen == IsSeen.Value);
            }

            if (!string.IsNullOrEmpty(NotifiedBy))
            {
                string notifiedByLower = NotifiedBy.ToLower();
                predicate = predicate.And(entry => entry.UserFrom.LastName != null && entry.UserFrom.LastName.ToLower().Contains(notifiedByLower));
            }
                
            return predicate;
        }
    }
}
