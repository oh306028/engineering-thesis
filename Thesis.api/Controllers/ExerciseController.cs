using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thesis.api.Modules.Answer.Details;
using Thesis.api.Modules.Answer.Update;
using Thesis.api.Modules.Exercise.Details;
using Thesis.app.Commands;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/exercise")]
    [ApiController]
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
        public ActionResult<List<ExerciseDetails>> GetExercises()
        {
            var query = new ExerciseQuery.GetList();
            var results = mediatR.Send(query);

            return Ok(mapper.Map<List<ExerciseDetails>>(results));
        }

        [HttpGet("student/{id}")]
        public ActionResult<List<ExerciseDetails>> GetStudentExercises(string id)  
        {
            var query = new ExerciseQuery.GetListByStudentId(id);
            var results = mediatR.Send(query);


            return Ok(mapper.Map<List<ExerciseDetails>>(results));
        }

        [HttpGet("{id}")]
        public ActionResult<List<ExerciseDetails>> GetExercise(string id)
        {
            var query = new ExerciseQuery.GetDetails(id); 
            var results = mediatR.Send(query);

            return Ok(mapper.Map<ExerciseDetails>(results));
        }

        [HttpPost("{exerciseId}/answer")]   
        public ActionResult Answer(string id, [FromBody]AnswerModel model)
        {
            var command = new ExerciseCommand.Answer(id, model);   
            mediatR.Send(command); 
              
            return Ok();
        }

        [HttpGet("{id}/answer")]    
        public ActionResult GetAnswer(string id) 
        {
            var query = new ExerciseQuery.GetAnswer(id);
            var result = mediatR.Send(query);   


            return Ok(mapper.Map<AnswerDetails>(result));
        }


    }
}
