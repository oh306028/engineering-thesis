using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Extensions;
using Thesis.app.Filters.AdminQueryFilters;
using Thesis.app.Pagination;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Thesis.app.Queries
{
    public class AdminQuery
    {
        public class Users : IRequest<PaginationResult<User>>
        {
            public UsersFilter Filter { get; set; } 
            public PaginationEntry Pagination { get; set; }

            public Users(UsersFilter filter, PaginationEntry pagination)
            {
                Filter = filter;
                Pagination = pagination;
            }
        }
            
        

        public class Classes : IRequest<PaginationResult<Classroom>>
        {
            public ClassFilter Filter { get; set; }
            public PaginationEntry Pagination { get; set; }

            public Classes(ClassFilter filter, PaginationEntry pagination)
            {
                Filter = filter;
                Pagination = pagination;    
            }
        }

        public class Loggins : IRequest<PaginationResult<LoginHistory>>
        {
            public LogginsFilter Filter { get; set; }
            public PaginationEntry Pagination { get; set; }

            public Loggins(LogginsFilter filter, PaginationEntry pagination)
            {
                Filter = filter;
                Pagination = pagination;
            }
        }

        public class LogginsPerStudent : IRequest<List<LoginHistory>>
        {
            public int ParentId { get; set; }
            public LogginsPerStudent(int parentId)
            {
                ParentId = parentId;
            }

        }


        public class TeacherAttempts : IRequest<List<Teacher>>
        {

        }    

    }

    public class GetTeacherAttemptsHandler : IRequestHandler<AdminQuery.TeacherAttempts, List<Teacher>>, IHandler
    {
        public AppDbContext DbContext { get; set; }
        private readonly IDistributedCache _cache;

        public GetTeacherAttemptsHandler(AppDbContext dbContext, IDistributedCache cache)
        {
            DbContext = dbContext;
            _cache = cache;
        }

        public async Task<List<Teacher>> Handle(AdminQuery.TeacherAttempts request, CancellationToken cancellationToken)
        {
            string cacheKey = "teacher_attempts_list";

            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<List<Teacher>>(cachedData);
            }

            var teachers = await DbContext.Users
                .OfType<Teacher>()
                .AsNoTracking()
                .Where(p => p.IsAccepted.HasValue && !p.IsAccepted.Value)
                .ToListAsync(cancellationToken);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            var serializedData = JsonConvert.SerializeObject(teachers, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions, cancellationToken);

            return teachers;
        }
    }

    public class GetUsersForAdmin : IRequestHandler<AdminQuery.Users, PaginationResult<User>>
    {
        private readonly AppDbContext DbContext;
        private readonly IDistributedCache _cache;

        public GetUsersForAdmin(AppDbContext dbContext, IDistributedCache cache)
        {
            DbContext = dbContext;
            _cache = cache;
        }

        public async Task<PaginationResult<User>> Handle(AdminQuery.Users request, CancellationToken cancellationToken)
        {
            string filterJson = JsonConvert.SerializeObject(request.Filter);
            string cacheKey = $"users_p{request.Pagination.PageNumber}_s{request.Pagination.PageSize}_{GenerateHash(filterJson)}";

            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<PaginationResult<User>>(cachedData);
            }

            var query = DbContext.Users
                .AsNoTracking()
                .Filter(request.Filter);

            var result = await query.ToPagedResultAsync(request.Pagination);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };

            var serializedResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await _cache.SetStringAsync(cacheKey, serializedResult, cacheOptions, cancellationToken);

            return result;
        }
        private string GenerateHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }

    public class GetClassesForAdmin : IRequestHandler<AdminQuery.Classes, PaginationResult<Classroom>>, IHandler
    {
        public AppDbContext DbContext { get; set; } 

        public GetClassesForAdmin(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<PaginationResult<Classroom>> Handle(AdminQuery.Classes request, CancellationToken cancellationToken)
        {

            var query = DbContext
                .Classrooms
                .Include(p => p.Teacher)
                .AsNoTracking()
                .Filter(request.Filter);

            return await query.ToPagedResultAsync(request.Pagination);

        }
    }

    public class GetLogginsForAdmin : IRequestHandler<AdminQuery.Loggins, PaginationResult<LoginHistory>>, IHandler
    {   
        public AppDbContext DbContext { get; set; }
        private readonly IDistributedCache _cache;

        public GetLogginsForAdmin(AppDbContext dbContext, IDistributedCache cache)
        {
            DbContext = dbContext;
            _cache = cache;
        }   

        public async Task<PaginationResult<LoginHistory>> Handle(AdminQuery.Loggins request, CancellationToken cancellationToken)
        {
            var filterJson = JsonConvert.SerializeObject(request.Filter);
            var cacheKey = $"logins_p{request.Pagination.PageNumber}_s{request.Pagination.PageSize}_{GenerateHash(filterJson)}";

            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<PaginationResult<LoginHistory>>(cachedData);
            }
            var query = DbContext.LoginHistories
                .Include(p => p.User)
                .AsNoTracking()
                .Filter(request.Filter)
                .OrderByDescending(p => p.LoginDate);

            var result = await query.ToPagedResultAsync(request.Pagination);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            };

            var serializedResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await _cache.SetStringAsync(cacheKey, serializedResult, cacheOptions, cancellationToken);

            return result;
        }

        private string GenerateHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
    public class GetLogginsPerStudent : IRequestHandler<AdminQuery.LogginsPerStudent, List<LoginHistory>>, IHandler
    {
        public AppDbContext DbContext { get; set; } 

        public GetLogginsPerStudent(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<List<LoginHistory>> Handle(AdminQuery.LogginsPerStudent request, CancellationToken cancellationToken)
        {
            var parent = await DbContext.Users.OfType<Parent>().Include(p => p.Students).FirstOrDefaultAsync(p => p.Id == request.ParentId);
            var loggins =  await DbContext.LoginHistories.Include(p => p.User).AsNoTracking().ToListAsync(cancellationToken);

            return loggins.Where(p => p.UserId == parent.Students.First().Id).OrderByDescending(p => p.LoginDate).ToList(); 

        }
    }
}
