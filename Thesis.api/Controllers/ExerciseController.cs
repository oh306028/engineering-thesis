using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Answer;
using Thesis.app.Dtos.Exercise;
using Thesis.app.Extensions;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public ExerciseController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<List<ExerciseDetails>>> GetExercises()
        {
            var query = new ExerciseQuery.GetList();
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<ExerciseDetails>>(results));
        }

        [HttpGet("student/{id}")]
        public async Task<ActionResult<List<ExerciseDetails>>> GetStudentExercises(string id)  
        {
            var query = new ExerciseQuery.GetListByStudentId(id);
            var results = await mediatR.Send(query);


            return Ok(mapper.Map<List<ExerciseDetails>>(results));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<ExerciseDetails>>> GetExercise(string id)
        {
            var query = new ExerciseQuery.GetDetails(id); 
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<ExerciseDetails>(results));
        }

        [HttpPost("{id}/answer")]   
        public async Task<ActionResult> Answer(string id, [FromBody]AnswerModel model)
        {
            var command = new ExerciseCommand.Answer(id, model, User.Id());   
            await mediatR.Send(command); 
              
            return Ok();
        }

        [HttpPost("{id}/game/{sessionId}/answer")]
        public async Task<ActionResult> GameAnswer(string id, Guid sessionId, [FromBody] AnswerModel model)
        {
            var command = new ExerciseCommand.GameAnswer(id, sessionId, model);
            await mediatR.Send(command);    

            return Ok();    
        }

        [HttpGet("{id}/answer")]    
        public async Task<ActionResult> GetAnswer(string id) 
        {
            var query = new ExerciseQuery.GetAnswer(id);
            var result = await mediatR.Send(query);   

            return Ok(mapper.Map<AnswerDetails>(result));
        }


    }
}
