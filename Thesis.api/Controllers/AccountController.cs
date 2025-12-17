using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Queries;
using Thesis.app.Dtos.Account;  
using Thesis.app.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Thesis.api.Controllers
{
    [Route("api/account")] 
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public AccountController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;

        }

        [HttpPost("register-teacher")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Register([FromForm] TeacherAccountRegisterModel model)
        {
            var command = new AccountCommand.RegisterTeacher(model);
            await mediatR.Send(command);   

            return Accepted();        
        }

        [HttpPost("register")]
        public async Task<ActionResult<AccountLoginModel>> Register([FromBody] AccountRegisterModel model)
        {
            var command = new AccountCommand.Register(model);
            var result = await mediatR.Send(command);

            return Accepted(result);
        }

        [HttpGet("impersonate-as-student")]
        [Authorize(Roles = "Parent")]
        public async Task<ActionResult<AccountSuccesLoginModel>> ImpersonateAsStudent()
        {
            var command = new AccountCommand.ImpersonateAsStudent(User.Id());
            var token = await mediatR.Send(command);

            return Accepted(token); 
        }

        [HttpGet("verify")]
        [Authorize]
        public ActionResult VerifyToken()
        {
            return Ok();
        }

        [HttpGet("my-profile")]
        [Authorize]

        public async Task<ActionResult<ProfileDetails>> GetProfileDetails()
        {
            var query = new AccountQuery.GetProfileDetails(User.Id());
            var result = await mediatR.Send(query); 

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccountSuccesLoginModel>> Login([FromBody] AccountLoginModel model)
        {
            var command = new AccountCommand.Login(model);
            var token = await mediatR.Send(command);

            return Ok(token);
        }

        [Authorize]
        [HttpGet("role")]
        public async Task<ActionResult<UserRoleModel>> GetUserRole()
        {
            var query = new AccountQuery.GetRole(User.Id());
            var result = await mediatR.Send(query);

            return Ok(result);
        }

    }
}
