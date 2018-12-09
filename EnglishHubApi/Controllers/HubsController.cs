using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnglishHubRepository;
using Microsoft.Extensions.Options;
using EnglishHubApi.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using MongoDB.Bson;
using EnglishHubRepository.Models;
using Newtonsoft.Json.Linq;

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
        // [HttpGet]
        // public ActionResult<IEnumerable<WordEntity>> GetAll()
        // {
        //     var result = hubRepository.GetAll().Result;
        //     return result;
        // }

        public Task<string> GetSentence(string word)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = false;
            client.DefaultRequestHeaders.Add("app_id", "4d31cebf");
            client.DefaultRequestHeaders.Add("app_key", "9c6555d3737099ff71385bfccc819494");
            HttpResponseMessage response = client.GetAsync("https://od-api.oxforddictionaries.com/api/v1/entries/en/" + word).Result;

            return response.Content.ReadAsStringAsync();
        }

        // GET api/values/5
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]WordRequestEntity word)
        {
            var result = await hubRepository.Add(new WordEntity
            {
                _id = ObjectId.GenerateNewId(),
                originalword = word.word,
                description = word.description,
                userId = word.userId,
                packageId = word.packageId,
                synonym = word.synonym,
                lexialCategory = word.lexialCategory,
                definition = word.definition,
                ownSentence = word.ownSentence
            });
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
        public async Task<bool> Delete([FromQuery]string id, [FromQuery]string packageId)
        {
            var result = await hubRepository.Remove(id, packageId);
            return result;
        }

        [HttpPost]
        public async Task<bool> Update([FromBody]WordRequestEntity entity)
        {
            var updatedEntity = new WordEntity
            {
                originalword = entity.word,
                packageId = entity.packageId,
                description = entity.description,
                ownSentence = entity.ownSentence,
                synonym = entity.synonym,
                _id = new MongoDB.Bson.ObjectId(entity.id)
            };
            var result = await hubRepository.Update(updatedEntity);
            return result;
        }

        public Task<List<Question>> GetQuestions(string packageId, int questionNumber = 20)
        {
            return hubRepository.QuestionEntities(packageId, questionNumber);
        }

        [HttpGet]
        public IEnumerable<WordEntity> GetWordsByUserId(string userId, string packageId)
        {
            var result = this.hubRepository.GetWordsByUserId(userId, packageId).Result;
            return result;
        }

        // [HttpGet]
        // public IEnumerable<PackageEntity> GetPackagesByUserId(string userId)
        // {
        //     var result = this.hubRepository.GetPackagesByUserId(userId).Result;
        //     return result;
        // }

        [HttpPost]
        public Task<PackageEntity> AddPackage(PackageEntity packageEntity)
        {
            packageEntity.words = new List<WordEntity>();
            var result = this.hubRepository.AddPackage(packageEntity);
            return result;
        }

        [HttpPost]
        public Task<bool> UpdatePackage(PackageEntityRequest request)
        {
            var result = this.hubRepository.UpdatePackage(new PackageEntity()
            {
                name = request.name,
                _id = new ObjectId(request._id),

            });
            return result;
        }

        [HttpPost]
        public async Task<bool> RemovePackage([FromQuery]string id)
        {
            var result = await hubRepository.RemovePackage(id);
            return result;
        }

        [HttpGet]
        public async Task<bool> FavoritePackage([FromQuery]string id, [FromQuery]bool status)
        {
            var result = await hubRepository.FavoritePackage(id, status);
            return result;
        }

        public Task<List<PackageEntity>> GetPackages(string id)
        {
            return hubRepository.GetPackagesByUserId(id);
        }

        public Task<List<Question>> QuestionEntities(string packageId, int questionNumber)
        {
            return hubRepository.QuestionEntities(packageId, questionNumber);
        }
    }
}
