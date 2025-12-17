using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.data.Interfaces;

namespace Thesis.app.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<T> Filter<T>(
        this IQueryable<T> query,   
            IFIlter<T> filter)
        {
            var predicate = filter.GetPredicate();

            return query.Where(predicate);
        }
    }
}
