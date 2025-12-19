using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.app.Dtos.LearningPath;
using Thesis.app.Queries;
using Thesis.data;
using Thesis.data.Enums;

namespace Thesis.api.Controllers
{
    [Route("api/dictionary")]
    [ApiController]
    [Authorize]
    public class DictionaryController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
            
        public DictionaryController(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;   
        }

        [HttpGet("subjects")]
        public ActionResult<List<KeyValuePair<string,string>>> GetSubjects()
        {
            return Ok(dbContext.Subjects.ToList().Select(p => new KeyValuePair<string, string>(p.PublicId.ToString(), p.Name)));
        }


        [HttpGet("badges")]
        public ActionResult<List<KeyValuePair<string, string>>> GetBadges()
        {
            return Ok(dbContext.Badges.Where(p => !p.LearningPathId.HasValue).ToList().Select(p => new KeyValuePair<string, string>(p.PublicId.ToString(), p.Emote)));
        }
    }
}
