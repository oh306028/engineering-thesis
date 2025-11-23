using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Account;
using Thesis.app.Dtos.Admin;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class AdminController : ControllerBase
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;
        public AdminController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;

        }
            
        [HttpGet("teacher-attempts")]
        public async Task<ActionResult<List<TeacherAttemptListModel>>> GetTeacherAttempts()
        {
            var query = new AdminQuery.TeacherAttempts();
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<List<TeacherAttemptListModel>>(result));
        }
            
        [HttpGet("users")]  
        public async Task<ActionResult<List<UserListModel>>> GetAllUsers()
        {
            var query = new AdminQuery.Users();
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<List<UserListModel>>(result));
        }

        [HttpGet("classes")]
        public async Task<ActionResult<List<ClassroomListModel>>> GetAllClasses()
        {
            var query = new AdminQuery.Classes();
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<List<ClassroomListModel>>(result));
        }

        [HttpGet("loggins")]
        public async Task<ActionResult<List<LogginHistoryListModel>>> GetLoggins()
        {
            var query = new AdminQuery.Loggins();    
            var result = await mediatR.Send(query);
                
            return Ok(mapper.Map<List<LogginHistoryListModel>>(result));  
        }


        [HttpPost("teacher-attempts/{id}/decline")]
        public async Task<ActionResult> DeclineTeacherAttempt(string id)
        {
            var command = new AdminCommand.ManageTeacherAttempt(id,false);
            await mediatR.Send(command);

            return Accepted();  
        }

        [HttpPost("teacher-attempts/{id}/accept")]
        public async Task<ActionResult> AcceptTeacherAttempt(string id)  
        {
            var command = new AdminCommand.ManageTeacherAttempt(id, true);
            await mediatR.Send(command);

            return Accepted();
        }
    }
}
