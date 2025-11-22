using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Classroom;
using Thesis.app.Extensions;
using Thesis.app.Queries;

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

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateClassroom(ClassroomCreateModel model)
        {
            var command = new ClassroomCommand.CreateClassroom(model, User.Id());
            await mediatR.Send(command);

            return Ok();
        }

        [Authorize(Roles = "Student")]
        [HttpPost("join")]
        public async Task<ActionResult> JoinClassroom([FromBody] JoinClassroomModel model)
        {
            var command = new ClassroomCommand.JoinClassroom(model, User.Id());
            await mediatR.Send(command);
                
            return Ok();
        }


        [Authorize(Roles = "Teacher, Admin")] 
        [HttpPost("{classroomId}/student/{id}/accept")]
        public async Task<ActionResult> AcceptStudent(string classroomId, string id)    
        {
            var command = new ClassroomCommand.AcceptStudent(classroomId, id, User.Id(), true);
            await mediatR.Send(command);

            return Ok();
        }

        [Authorize(Roles = "Teacher, Admin")]
        [HttpPost("{classroomId}/student/{id}/decline")]
        public async Task<ActionResult> DeclineStudent(string classroomId, string id)
        {
            var command = new ClassroomCommand.AcceptStudent(classroomId, id, User.Id(), false);
            await mediatR.Send(command);

            return Ok();
        }

        //TO DO:
        //panel tworzenia zadan domowych, powtorek itd
        //panel powiadomien, wiadomosci
        //panel listy studentow

        //z poziomu ucznia =>
        //koledzy z klasy
        //widok drabinki (czyli najwiecej rozwiazanych zadan itd)

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassroomDetails>> GetDetails(string id)
        {

            var query = new ClassroomQuery.GetDetails(id);
            var result = await mediatR.Send(query);
            return Ok(result);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<ActionResult<ClassroomList>> GetList()    
        {

            var query = new ClassroomQuery.GetList(User.Id());  
            var result = await mediatR.Send(query);
            return Ok(result);
        }



    }
}
