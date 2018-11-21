using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EnglishHubRepository
{
    public class AnswerRepository : IAnswerRepository
    {
        private WordContext context;
        public AnswerRepository(string connectionString, string databaseName)
        {
            context = new WordContext(connectionString, databaseName);
        }

        public async Task<AnswerEntity> Add(AnswerEntity entity)
        {
            await this.context.Answers.InsertOneAsync(entity);
            return entity;
        }

        public async Task<List<AnswerEntity>> AddMany(List<AnswerEntity> entities)
        {
            await this.context.Answers.InsertManyAsync(entities);
            return entities;
        }

        public Task<AnswerEntity> Get(string id)
        {
            // var filter = Builders<WordEntity>.Filter.Eq("_id", id);
            return this.context.Answers.Find<AnswerEntity>(new BsonDocument { { "_id", new ObjectId(id) } }).FirstOrDefaultAsync();
        }

        public Task<List<AnswerEntity>> GetAll()
        {
            var result = this.context.Answers.Find<AnswerEntity>(new BsonDocument()).ToListAsync();
            return result;
        }

        public async Task<List<AnswerEntity>> GetResults()
        {
            var agg = this.context.Answers.Aggregate()
                .Group(BsonDocument.Parse("{ _id: '$Word', myCount: { $sum: 1 } }")).ToList().Select(x => new AnswerEntity { Word = x["_id"].ToString(), Attempt = (int)x["myCount"] }).ToList();

            return agg;
        }

        public async Task<bool> Remove(string id)
        {
            var filter = Builders<AnswerEntity>.Filter.Eq("_id", id);
            var result = await this.context.Answers.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> RemoveAll()
        {
            var result = await this.context.Answers.DeleteManyAsync(new BsonDocument());

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> Update(AnswerEntity entity)
        {
            return true;
        }
    }
}
