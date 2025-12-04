using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.app.Dtos.Notification;
using Thesis.data;
using Thesis.data.Data;
using Thesis.data.Interfaces;

namespace Thesis.app.Queries
{
    public class NotificationQuery
    {
        public class GetUserNotifications : IRequest<List<Notification>> 
        {
            public int UserId { get; set; }
            public GetUserNotifications(int userId)
            {
                UserId = userId;
            }

        }
        public class GetMessageTypes : IRequest<List<NotificationMessage>>
        {

        }

        public class GetNotificationsFromParents : IRequest<List<Notification>>
        {
            public string ClassroomId { get; set; }
            public int UserId { get; set; } 
            public GetNotificationsFromParents(int userId, string classroomId)
            {
                UserId = userId;
                ClassroomId = classroomId;
            }   
        }

        public class GetNotificationsFromStudents : IRequest<List<Notification>>
        {   
            public string ClassroomId { get; set; }
            public int UserId { get; set; } 
            public GetNotificationsFromStudents(int userId, string classroomId)
            {
                UserId = userId;
                ClassroomId = classroomId;
            }
        }

            

        public class GetNotificatioDetails : IRequest<Notification>
        {
            public int UserId { get; set; }
            public string PublicId { get; set; }    
            public GetNotificatioDetails(int userId, string publicId)
            {
                UserId = userId;
                PublicId = publicId;
            }

        }

    }   

    public class GetParentsNotificationsHandler : IRequestHandler<NotificationQuery.GetNotificationsFromParents, List<Notification>>, IHandler
    {
        public AppDbContext DbContext { get; set; }


        public GetParentsNotificationsHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;  
        }
        public async Task<List<Notification>> Handle(NotificationQuery.GetNotificationsFromParents request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Users
                .Include(p => p.RecivedNotifications).ThenInclude(p => p.Message)
                .Include(p => p.RecivedNotifications).ThenInclude(p => p.UserFrom)
                .AsNoTracking()
                .SingleAsync(p => p.Id == request.UserId);

            var classroom = await DbContext.Classrooms.Include(p => p.Students).FirstOrDefaultAsync(p => p.PublicId == Guid.Parse(request.ClassroomId));

            var classroomStudentIds = classroom.Students.Select(p => p.Id).ToList();

            var parents =  DbContext.Users.OfType<Parent>().Where(p => p.Students.Any(p => classroomStudentIds.Contains(p.Id))).Select(p => p.Id).ToList();

            return user.RecivedNotifications.Where(p => !p.IsSystemNotification && parents.Contains((int)p.UserFromId)).ToList();   

        }
    }

    public class GetUserNotification : IRequestHandler<NotificationQuery.GetUserNotifications, List<Notification>>, IHandler
    {
        public AppDbContext DbContext { get; set; } 


        public GetUserNotification(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<List<Notification>> Handle(NotificationQuery.GetUserNotifications request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Users
                .Include(p => p.RecivedNotifications).ThenInclude(p => p.Message)
                .Include(p => p.RecivedNotifications).ThenInclude(p => p.UserFrom)
                .AsNoTracking()
                .SingleAsync(p => p.Id == request.UserId);

            return user.RecivedNotifications;

        }
    }

    public class GetStudentsNotificationsHandler : IRequestHandler<NotificationQuery.GetNotificationsFromStudents, List<Notification>>, IHandler
    {
        public AppDbContext DbContext { get; set; }


        public GetStudentsNotificationsHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }   
        public async Task<List<Notification>> Handle(NotificationQuery.GetNotificationsFromStudents request, CancellationToken cancellationToken)
        {
            var user = await DbContext.Users
                 .Include(p => p.RecivedNotifications).ThenInclude(p => p.Message)
                 .Include(p => p.RecivedNotifications).ThenInclude(p => p.UserFrom)
                 .AsNoTracking()
                 .SingleAsync(p => p.Id == request.UserId);

            var classroom = await DbContext.Classrooms.Include(p => p.Students).FirstOrDefaultAsync(p => p.PublicId == Guid.Parse(request.ClassroomId));

            var classroomStudentIds = classroom.Students.Select(p => p.Id).ToList();

            return user.RecivedNotifications.Where(p => !p.IsSystemNotification && classroomStudentIds.Contains((int)p.UserFromId)).ToList();

        }
    }

    public class GetMessageTypessHandler : IRequestHandler<NotificationQuery.GetMessageTypes, List<NotificationMessage>>, IHandler
    {
        public AppDbContext DbContext { get; set; }


        public GetMessageTypessHandler(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<List<NotificationMessage>> Handle(NotificationQuery.GetMessageTypes request, CancellationToken cancellationToken)
        {
            return await DbContext.NotificationMessages.ToListAsync();

        }
    }

   
}
