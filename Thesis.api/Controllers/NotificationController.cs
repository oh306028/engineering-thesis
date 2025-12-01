using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Dtos.Badge;
using Thesis.app.Dtos.Notification;
using Thesis.app.Extensions;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/notification")]
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
        public async Task<ActionResult<List<NotificationList>>> GetCurrentUserNotifications()  
        {
            var query = new NotificationQuery.GetUserNotifications(User.Id());
            var results = await mediatR.Send(query);          

            return Ok(mapper.Map<List<NotificationList>>(results));
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
