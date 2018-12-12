using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using EnglishHubRepository.Models;

namespace EnglishHubRepository
{
    public class WordRepository : IWordRepository
    {
        private WordContext context;
        public WordRepository(string connectionString, string databaseName)
        {
            context = new WordContext(connectionString, databaseName);
        }

        public async Task<bool> Add(WordEntity entity)
        {
            var filter = Builders<PackageEntity>.Filter.Eq("_id", new ObjectId(entity.packageId));
            var update = Builders<PackageEntity>.Update.Push("words", entity);

            var result = await this.context.Packages.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public Task<WordEntity> Get(string id)
        {
            // var filter = Builders<WordEntity>.Filter.Eq("_id", id);
            return this.context.Words.Find<WordEntity>(new BsonDocument { { "_id", new ObjectId(id) } }).FirstOrDefaultAsync();
        }

        public async Task<List<WordEntity>> GetWordsByUserId(string userId, string packageId)
        {
            var builder = Builders<PackageEntity>.Filter;
            var filter = (builder.Eq("userId", userId) & builder.Eq("_id", new ObjectId(packageId)));
            var result = await this.context.Packages.Find<PackageEntity>(filter).ToListAsync();

            var r = result.Select(x => x.words).FirstOrDefault();
            return r;
        }

        public async Task<List<WordEntity>> GetAll(string packageId)
        {
            var builder = Builders<PackageEntity>.Filter;
            var filter = builder.Eq("_id", new ObjectId(packageId));
            var result = await this.context.Packages.Find<PackageEntity>(filter).ToListAsync();

            var r = result.Select(x => x.words).FirstOrDefault();
            return r;
        }

        public async Task<bool> Remove(string id, string packageId)
        {
            var result = await this.context.Packages.UpdateOneAsync(
     Builders<PackageEntity>.Filter.Where(x => x._id == ObjectId.Parse(packageId)),
     Builders<PackageEntity>.Update.PullFilter(x => x.words, y => y._id == ObjectId.Parse(id)));
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> RemoveAll()
        {
            var result = await this.context.Words.DeleteManyAsync(new BsonDocument());

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> Update(WordEntity entity)
        {
            // var filter = Builders<WordEntity>.Filter.Eq("_id", entity._id);
            // var update = Builders<WordEntity>.Update
            // .Set(s => s.originalword, entity.originalword)
            // .Set(s => s.synonym, entity.synonym)
            // .Set(s => s.ownSentence, entity.ownSentence)
            // .Set(s => s.description, entity.description);

            // var result = await this.context.Words.UpdateOneAsync(filter, update);

            var result= await this.context.Packages.FindOneAndUpdateAsync(
    c => c._id == ObjectId.Parse(entity.packageId) && c.words.Any(s => s._id == entity._id), // find this match
    Builders<PackageEntity>.Update.Set(c => c.words[-1].originalword, entity.originalword)
             //.Set(s => s.synonym, entity.synonym)
            // .Set(s => s.ownSentence, entity.ownSentence)
             .Set(s => s.words[-1].description, entity.description));

            return true;//result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<PackageEntity> AddPackage(PackageEntity entity)
        {
            await this.context.Packages.InsertOneAsync(entity);
            return entity;
        }

        public async Task<bool> UpdatePackage(PackageEntity entity)
        {
            var filter = Builders<PackageEntity>.Filter.Eq("_id", entity._id);
            var update = Builders<PackageEntity>.Update
            .Set(s => s.name, entity.name);

            var result = await this.context.Packages.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> RemovePackage(string id)
        {
            var filter = Builders<PackageEntity>.Filter.Eq("_id", new ObjectId(id));

            var result = await this.context.Packages.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<List<PackageEntity>> GetPackagesByUserId(string userId)
        {
            var filter = Builders<PackageEntity>.Filter.Eq("userId", userId);

            var results = await this.context.Packages.Find<PackageEntity>(filter).ToListAsync();


            return results;
        }

        public async Task<bool> FavoritePackage(string id, int starCount)
        {
            var filter = Builders<PackageEntity>.Filter.Eq("_id", new ObjectId(id));
            var update = Builders<PackageEntity>.Update
            .Set(s => s.starCount, starCount);

            var result = await this.context.Packages.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<List<Question>> QuestionEntities(string packageId, int questionNumber = 20)
        {
            var filter = Builders<PackageEntity>.Filter.Eq("_id", new ObjectId(packageId));
            var projection = Builders<PackageEntity>.Projection.Include("words");
            var result = await this.context.Packages.Aggregate().Match(filter).Project<PackageEntity>(projection).ToListAsync();
            var r = result.FirstOrDefault();

            var wordids = r.words.Select(x => x._id.ToString()).ToList();

            Question question2 = new Question();
            var randomWordIds = question2.GetRandomWords(wordids, questionNumber);



            var options = r.words.Select(x => x.description).ToList();
            var newQuestionWords = r.words.Where(x => randomWordIds.Contains(x._id.ToString())).ToList();
            var questions = new List<Question>();

            for (int i = 0; i < newQuestionWords.Count; i++)
            {
                var question = new Question();
                question.QuestionWord = newQuestionWords[i].originalword;
                question.Answer = newQuestionWords[i].description;
                question.PrepareOptions(options.Where(x => x != question.Answer).ToList());
                questions.Add(question);
            }

            return questions;
        }
    }
}
