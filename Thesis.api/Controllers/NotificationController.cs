using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Dtos.Badge;
using Thesis.app.Dtos.Notification;
using Thesis.app.Extensions;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/notification")]
    [ApiController]
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
        public async Task<ActionResult<List<BadgeDetails>>> GetCurrentUserNotifications()  
        {
            var query = new NotificationQuery.GetUserNotifications(User.Id());
            var results = await mediatR.Send(query);          

            return Ok(mapper.Map<List<NotificationList>>(results));
        }

    }
}
