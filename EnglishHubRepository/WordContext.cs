using MongoDB.Driver;
using MongoDB.Bson;

namespace EnglishHubRepository
{
    public class WordContext
    {
        private IMongoDatabase _database = null;
        public WordContext(string connectionString, string database)
        {
            IMongoClient client = new MongoClient(connectionString);
            if (client != null)
            {
                _database = client.GetDatabase(database);
            }
        }

        public IMongoCollection<BsonDocument> GetDocument(string name)
        {
            return _database.GetCollection<BsonDocument>(name);
        }

        public IMongoCollection<WordEntity> Words
        {
            get{
                return _database.GetCollection<WordEntity>("hubs");
            }
        } 

        public IMongoCollection<AnswerEntity> Answers
        {
            get{
                return _database.GetCollection<AnswerEntity>("answers");
            }
        } 

        public IMongoCollection<PackageEntity> Packages
        {
            get{
                return _database.GetCollection<PackageEntity>("packages");
            }
        } 
    }
}