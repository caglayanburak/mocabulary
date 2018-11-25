using MongoDB.Bson;

namespace EnglishHubRepository
{
    public class WordEntity
    {
        public ObjectId _id { get; set; }
        public string originalword { get; set; }
        public string description { get; set; }
        public string userId { get; set; }
        public string packageId { get; set; }
    }
}