using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Filters.AdminQueryFilters
{
    public class LogginsFilter : IFIlter<LoginHistory>
    {
        public DateTime? LoginDateFrom { get; set; }
        public DateTime? LoginDateTo { get; set; }
        public bool? IsSucceeded { get; set; }

        public Expression<Func<LoginHistory, bool>> GetPredicate()
        {
            var predicate = PredicateBuilder.New<LoginHistory>(true);

            if (LoginDateFrom.HasValue)
            {
                Expression<Func<LoginHistory, bool>> dateFromFilter =
                    entry => entry.LoginDate >= LoginDateFrom.Value;

                predicate = predicate.And(dateFromFilter);
            }

            if (LoginDateTo.HasValue)
            {
                Expression<Func<LoginHistory, bool>> dateToFilter =
                    entry => entry.LoginDate <= LoginDateTo.Value;

                predicate = predicate.And(dateToFilter);
            }

            if (IsSucceeded.HasValue)
            {
                predicate = predicate.And(entry => entry.IsSucceeded == IsSucceeded.Value);
            }

            return predicate;
        }
    }

}
