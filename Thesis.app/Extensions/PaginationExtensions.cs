using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Pagination;

namespace Thesis.app.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PaginationResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            PaginationEntry pagination) where T : class
        {
            var totalCount = await query.CountAsync();

            var skip = (pagination.PageNumber - 1) * pagination.PageSize;

            var items = await query
                .Skip(skip)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PaginationResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}
