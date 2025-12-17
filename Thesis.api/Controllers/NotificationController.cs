using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Badge;
using Thesis.app.Dtos.Notification;
using Thesis.app.Extensions;
using Thesis.app.Filters.AdminQueryFilters;
using Thesis.app.Filters.NotificationQueryFilters;
using Thesis.app.Pagination;
using Thesis.app.Queries;
using Thesis.data.Enums;

namespace Thesis.api.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {

        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public NotificationController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationDetails>>> GetCurrentUserNotifications()  
        {
            var query = new NotificationQuery.GetUserNotifications(User.Id());
            var results = await mediatR.Send(query);          

            return Ok(mapper.Map<List<NotificationDetails>>(results));
        }

        [HttpGet("{classroomId}/from-parents")]
        public async Task<ActionResult<PagedNotificationDetails>> GetNotificationsFromParents(string classroomId, [FromQuery] NotificationListFilter filter, [FromQuery] PaginationEntry pagination)    
        {
            var query = new NotificationQuery.GetNotificationsFromParents(User.Id(), classroomId, filter, pagination);   
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<PagedNotificationDetails>(results));
        }

        [HttpGet("{classroomId}/from-students")]
        public async Task<ActionResult<PagedNotificationDetails>> GetNotificationsFromStudents(string classroomId, [FromQuery] NotificationListFilter filter, [FromQuery] PaginationEntry pagination)
        {   
            var query = new NotificationQuery.GetNotificationsFromStudents(User.Id(), classroomId, filter, pagination);
            var results = await mediatR.Send(query);
                
            return Ok(mapper.Map<PagedNotificationDetails>(results));
        }

        [HttpGet("message-dictionary")]
        public async Task<ActionResult<List<MessageType>>> GetMessagesDictionary()
        {
            var query = new NotificationQuery.GetMessageTypes();
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<MessageType>>(results));
        }

        [HttpPost("send")]
        public async Task<ActionResult> SendNotification([FromBody]NotificationModel model)
        {       
            var query = new NotificationCommand.SendNotification(model, User.Id());
            await mediatR.Send(query);

            return Accepted();  
        }

        [HttpPost("{id}/mark-as-seen")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> MarkAsSeen(Guid id)
        {   
            var command = new NotificationCommand.MarkAsSeen(id, User.Id());
            await mediatR.Send(command);    

            return Accepted();  
        }

    }
}
