using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Badge;
using Thesis.app.Dtos.Notification;
using Thesis.app.Extensions;
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
        public async Task<ActionResult<List<NotificationDetails>>> GetNotificationsFromParents(string classroomId)    
        {
            var query = new NotificationQuery.GetNotificationsFromParents(User.Id(), classroomId);   
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<NotificationDetails>>(results));
        }

        [HttpGet("{classroomId}/from-students")]
        public async Task<ActionResult<List<NotificationDetails>>> GetNotificationsFromStudents(string classroomId)
        {   
            var query = new NotificationQuery.GetNotificationsFromStudents(User.Id(), classroomId);
            var results = await mediatR.Send(query);
                
            return Ok(mapper.Map<List<NotificationDetails>>(results));
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

        //[HttpPost]  
        //public async Task<ActionResult> SendNotification(NotificationModel model)
        //{
        //    var command = new NotificationCommand.SendNotification(User.Id());
        //    await mediatR.Send(command);

        //    return Accepted();  
        //}

        //[HttpGet("types")]
        //public async Task<ActionResult<List<BadgeDetails>>> GetCurrentUserNotifications()
        //{
        //    var query = new NotificationQuery.GetUserNotifications(User.Id());
        //    var results = await mediatR.Send(query);

        //    return Ok(mapper.Map<List<NotificationList>>(results));
        //}
    }
}
