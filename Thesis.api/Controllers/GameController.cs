using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Exercise;
using Thesis.app.Dtos.Game;
using Thesis.app.Extensions;
using Thesis.app.Queries;
using Thesis.data;

namespace Thesis.api.Controllers
{
    [Route("api/games")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IMediator mediatR;

        public GameController(AppDbContext dbContext, IMapper mapper, IMediator mediatR)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.mediatR = mediatR;
        }


        [HttpPost("start")]
        public async Task<ActionResult<Guid>> StartSession()
        {
            var command = new GameCommand.StartSession(User.Id());
            var result = await mediatR.Send(command);

            return Accepted(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseDetails>> GetQuestion(Guid id)
        {
            var query = new GameQuery.GetQuestion(id);
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<ExerciseDetails>(result)); 
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<GameDetails>> GetGameDetails(Guid id)
        {
            var query = new GameQuery.GetDetails(id);
            var result = await mediatR.Send(query);

            return Ok(mapper.Map<GameDetails>(result));
        }   
    }

}
