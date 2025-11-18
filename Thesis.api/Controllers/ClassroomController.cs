using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos;
using Thesis.app.Extensions;

namespace Thesis.api.Controllers
{
    [Route("api/classroom")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public ClassroomController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;

        }   

        public async Task<ActionResult> CreateClassroom(ClassroomCreateModel model)
        {
            var command = new ClassroomCommand.CreateClassroom(model, User.Id());
            await mediatR.Send(command);

            return Ok();
        }     



    }
}
