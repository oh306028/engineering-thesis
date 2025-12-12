using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Commands;
using Thesis.app.Dtos.Badge;
using Thesis.app.Extensions;
using Thesis.app.Queries;
using Thesis.data.Data;

namespace Thesis.api.Controllers
{
    [Route("api/rewards")]
    [ApiController]
    [Authorize]
    public class RewardController : ControllerBase
    {   
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public RewardController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;
                
        }

        [HttpGet("{learningPathId}")]   
        public async Task<ActionResult<BadgeDetails>> GetBadgeByLearningPath(string learningPathId)      
        {
            var query = new BadgeQuery.GetListByPath(learningPathId);     
            var results = await mediatR.Send(query);  

            return Ok(mapper.Map<BadgeDetails>(results));   
        }

        [HttpGet("new-rewards")]
        public async Task<ActionResult<bool>> AreAnyNewRewards()
        {   
            var query = new BadgeQuery.AreAnyNewRewards(User.Id());
            var result = await mediatR.Send(query);

            return Ok(result);
        }

        [HttpPost("mark-as-seen")]
        public async Task<ActionResult<bool>> MarkAsSeen()  
        {
            var command = new BadgeCommand.MarkAsSeen(User.Id()); 
            await mediatR.Send(command); 
                
            return Accepted();  
        }

        [HttpGet("mine-badges")]    
        public async Task<ActionResult<List<BadgeDetails>>> GetMineBadges() 
        {
            var query = new BadgeQuery.GetListForStudent(User.Id());  
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<BadgeDetails>>(results));
        }

        [HttpGet("achievements")]
        public async Task<ActionResult<List<AchievementDetails>>> GetAchievements()
        {
            var query = new BadgeQuery.GetAchievements();
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<AchievementDetails>>(results));
        }   

        [HttpGet("mine-achievements")]
        public async Task<ActionResult<List<AchievementDetails>>> GetMineAchievements()
        {
            var query = new BadgeQuery.GetMineAchievements(User.Id());
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<AchievementDetails>>(results));
        }





    }
}
