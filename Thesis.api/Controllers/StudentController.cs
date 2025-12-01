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
    [Authorize(Roles = "Student")]
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
        public async Task<ActionResult<StudentProgressDetails>> GetStudentProgress([FromQuery] int currentLevel)
        {
            var query = new StudentQuery.GetProgress(User.Id(), currentLevel);
            var result = await mediatR.Send(query);

            return Ok(result);
        }


    }
}
