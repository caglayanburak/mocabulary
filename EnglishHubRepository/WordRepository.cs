using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

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

        public async Task<bool> Remove(string id)
        {
            var filter = Builders<WordEntity>.Filter.Eq("_id", new ObjectId(id));

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
            .Set(s => s.synonym, entity.synonym)
            .Set(s => s.ownSentence, entity.ownSentence)
            .Set(s => s.description, entity.description);

            var result = await this.context.Words.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
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

        public async Task<bool> FavoritePackage(string id, bool status)
        {
            var filter = Builders<PackageEntity>.Filter.Eq("_id", new ObjectId(id));
            var update = Builders<PackageEntity>.Update
            .Set(s => s.isFavorite, status);

            var result = await this.context.Packages.UpdateOneAsync(filter, update);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
