using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Account;

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

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] AccountRegisterModel model)
        {
            var command = new AccountCommand.Register(model);
            await mediatR.Send(command);

            return Accepted();
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] AccountLoginModel model)
        {
            var command = new AccountCommand.Login(model);
            var token = await mediatR.Send(command);

            return Ok(token);
        }

    }
}
