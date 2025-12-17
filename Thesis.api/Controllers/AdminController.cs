using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Account;
using Thesis.app.Dtos.Admin;
using Thesis.app.Extensions;
using Thesis.app.Filters.AdminQueryFilters;
using Thesis.app.Pagination;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/admin")]
    [ApiController]

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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TeacherAttemptListModel>>> GetTeacherAttempts()
        {
            var query = new AdminQuery.TeacherAttempts();
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<List<TeacherAttemptListModel>>(result));
        }
            
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedUserListModel>> GetAllUsers([FromQuery] UsersFilter filter, [FromQuery] PaginationEntry pagination)
        {
            var query = new AdminQuery.Users(filter, pagination);
            var pagedResult = await mediatR.Send(query);

            var resultDto = mapper.Map<PagedUserListModel>(pagedResult);    

            return Ok(resultDto);
        }

        [HttpGet("classes")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedClassroomListModel>> GetAllClasses([FromQuery] ClassFilter filter, [FromQuery] PaginationEntry pagination)
        {
            var query = new AdminQuery.Classes(filter, pagination);
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<PagedClassroomListModel>(result));
        }

        [HttpGet("loggins")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedLogginHistoryListModel>> GetLoggins([FromQuery] LogginsFilter filter, [FromQuery] PaginationEntry pagination)
        {   
            var query = new AdminQuery.Loggins(filter, pagination);
            var pagedResult = await mediatR.Send(query);

            var resultDto = mapper.Map<PagedLogginHistoryListModel>(pagedResult);

            return Ok(resultDto);
        }

        [Authorize(Roles = "Admin, Parent")]
        [HttpGet("loggins/student")]
        public async Task<ActionResult<List<LogginHistoryListModel>>> GetLogginsForStudent()
        {
            var query = new AdminQuery.LogginsPerStudent(User.Id());   
            var result = await mediatR.Send(query);
                
            return Ok(mapper.Map<List<LogginHistoryListModel>>(result));
        }


        [HttpPost("teacher-attempts/{id}/decline")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeclineTeacherAttempt(string id)
        {
            var command = new AdminCommand.ManageTeacherAttempt(id,false);
            await mediatR.Send(command);

            return Accepted();  
        }

        [HttpPost("teacher-attempts/{id}/accept")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AcceptTeacherAttempt(string id)  
        {
            var command = new AdminCommand.ManageTeacherAttempt(id, true);
            await mediatR.Send(command);

            return Accepted();
        }
    }
}
