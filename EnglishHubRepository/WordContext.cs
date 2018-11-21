using MongoDB.Driver;

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
    }
}