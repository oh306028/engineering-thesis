using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Dtos.Exercise;
using Thesis.app.Dtos.LearningPath;
using Thesis.app.Dtos.Resource;
using Thesis.app.Extensions;
using Thesis.app.Queries;
using Thesis.data.Data;

namespace Thesis.api.Controllers
{
    [Route("api/learning-path")]
    [ApiController]
    [Authorize]
    public class LearningPathController : ControllerBase
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public LearningPathController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<LearningPathDetails>>> GetPaths()   
        {
            var query = new LearningPathQuery.GetList();    
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<LearningPathDetails>>(results));  
        }

        //endpoint returns publicId and flag IsDone for current student that watch those exercises
        [HttpGet("{id}/exercise")]
        public async Task<ActionResult<PathExercisesResource>> GetPathExercise(string id)          
        {
            var query = new LearningPathQuery.GetExercises(id);   
            var results = await mediatR.Send(query);

            var mapped = mapper.Map<List<PathExercise>>(results);


            return Ok(new PathExercisesResource(mapped));    
        }   







    }
}
