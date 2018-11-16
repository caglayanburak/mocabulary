using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EnglishHubRepository
{
    public class WordRepository : IWordRepository
    {
        private WordContext context;
        public WordRepository(string connectionString, string databaseName)
        {
            context = new WordContext(connectionString, databaseName);
        }

        public async Task<WordEntity> Add(WordEntity entity)
        {
            if(string.IsNullOrEmpty(entity.description) || string.IsNullOrEmpty(entity.originalword))
            {
                throw new ArgumentNullException("At least one of word or menaning is filled");
            }
            
            var filter = Builders<WordEntity>.Filter.Eq("originalword", entity.originalword);
            var hasAddedWord = await this.context.Words.Find(filter).FirstOrDefaultAsync();
            if (hasAddedWord != null)
            {
                return hasAddedWord;
            }
            await this.context.Words.InsertOneAsync(entity);
            return entity;
        }

        public Task<WordEntity> Get(string id)
        {
            // var filter = Builders<WordEntity>.Filter.Eq("_id", id);
            return this.context.Words.Find<WordEntity>(new BsonDocument { { "_id", new ObjectId(id) } }).FirstOrDefaultAsync();
        }

        public Task<List<WordEntity>> GetAll()
        {
            var result = this.context.Words.Find<WordEntity>(new BsonDocument()).ToListAsync();
            return result;
        }

        public async Task<bool> Remove(string id)
        {
            var filter = Builders<WordEntity>.Filter.Eq("_id", id);
            var result = await this.context.Words.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> RemoveAll()
        {
            var result = await this.context.Words.DeleteManyAsync(new BsonDocument());

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<bool> Update(WordEntity entity)
        {
            var filter = Builders<WordEntity>.Filter.Eq("_id", entity._id);
            var update = Builders<WordEntity>.Update
            .Set(s => s.originalword, entity.originalword)
            .Set(s => s.description, entity.description);

            var result = await this.context.Words.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
