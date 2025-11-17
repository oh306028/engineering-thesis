using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Dtos.Badge;
using Thesis.app.Queries;

namespace Thesis.api.Controllers
{
    [Route("api/badge")]
    [ApiController]
    public class BadgeController : ControllerBase
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        public BadgeController(IMediator mediatR, IMapper mapper)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;

        }

        [HttpGet("{learningPathId}")]   
        public async Task<ActionResult<List<BadgeDetails>>> GetBadgesByLearningPath(string learningPathId)      
        {
            var query = new BadgeQuery.GetListByPath();     
            var results = await mediatR.Send(query);  

            return Ok(mapper.Map<List<BadgeDetails>>(results));
        }

        [HttpGet("{studentId}")]    
        public async Task<ActionResult<List<BadgeDetails>>> GetStudentBadges(string studentId)
        {
            var query = new BadgeQuery.GetListForStudent();  
            var results = await mediatR.Send(query);

            return Ok(mapper.Map<List<BadgeDetails>>(results));
        }





    }
}
