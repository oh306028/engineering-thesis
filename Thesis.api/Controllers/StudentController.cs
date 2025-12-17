using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Dtos.Student;
using Thesis.app.Extensions;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/student")]
    [ApiController]
    
    public class StudentController : ControllerBase
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public StudentController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;

        }

        //na froncie przetrzymywany jest currentLevel, bedziemy wysylac go do endpointu po wykonaniu zadania i sprawdzac czy ulegl zmianie by wyswietlil odpowiedni komunikat

        [HttpGet("progress")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<StudentProgressDetails>> GetStudentProgress([FromQuery] int level)
        {
            var query = new StudentQuery.GetProgress(User.Id(), level);
            var result = await mediatR.Send(query);

            return Ok(result);
        }

        [HttpGet("for-parent")]
        [Authorize(Roles = "Parent")]
        public async Task<ActionResult<StudentDetailsWithClassroom>> GetStudentInfoForParent()   
        {
            var query = new StudentQuery.GetStudentInfoForParent(User.Id());
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<StudentDetailsWithClassroom>(result));
        }
            
    }
}
