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
    public class HubController : ControllerBase
    {
        private IWordRepository hubRepository;
        public HubController(IWordRepository _wordRepository, IOptions<Settings> settings)
        {
            this.hubRepository = _wordRepository;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<WordEntity>> GetAll()
        {
            var result = hubRepository.GetAll().Result;
            return result;
        }

        // GET api/values/5
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]WordRequestEntity word)
        {
            var result = await hubRepository.Add(new WordEntity { originalword = word.word, description = word.description });
            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public void DeleteCollection()
        {
            hubRepository.RemoveAll();
        }

        // PUT api/values/5
        [HttpPost]
        public async Task<bool> Delete([FromQuery]string id)
        {
            var result = await hubRepository.Remove(id);
            return result;
        }

        [HttpPost]
        public async Task<bool> Update([FromBody]WordRequestEntity entity)
        {
            var updatedEntity = new WordEntity
            {
                originalword = entity.word,
                description = entity.description,
                _id = new MongoDB.Bson.ObjectId(entity.id)
            };
            var result = await hubRepository.Update(updatedEntity);
            return result;
        }

        public ActionResult<IEnumerable<Question>> GetQuestions()
        {
            var result = hubRepository.GetAll().Result.ToList();

            var options = result.Select(x => x.description).ToList();
            var questions = new List<Question>();

            for (int i = 0; i < result.Count; i++)
            {
                var question = new Question();
                question.QuestionWord = result[i].originalword;
                question.Answer = result[i].description;
                question.PrepareOptions(options.Where(x => x != question.Answer).ToList());
                questions.Add(question);
            }

            return questions;
        }

        
    }
}
