using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishHubRepository;
using Microsoft.Extensions.Options;
using EnglishHubApi.Models;

namespace EnglishHubApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private IAnswerRepository answerRepository;
        public AnswerController(IAnswerRepository _answerRepository, IOptions<Settings> settings)
        {
            this.answerRepository = _answerRepository;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<AnswerEntity>> GetAll()
        {
            var result = answerRepository.GetAll().Result;
            return result;
        }

        // GET api/values/5
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]AnswerEntity answer)
        {
            var result = await answerRepository.Add(answer);
            return Ok(result);
        } 

        public Task<List<AnswerEntity>> GetResults()
        {
            return answerRepository.GetResults();
        }

        [HttpPost]
        public async Task<IActionResult> AddMany([FromBody]List<AnswerEntity> answers)
        {
            var result = await answerRepository.AddMany(answers);
            return Ok(result);
        }        
    }
}
