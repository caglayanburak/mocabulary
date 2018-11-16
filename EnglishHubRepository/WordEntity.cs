using MongoDB.Bson;

namespace EnglishHubRepository
{
    public class WordEntity
    {
        public ObjectId _id { get; set; }
        public string originalword { get; set; }
        public string description { get; set; }
    }
}