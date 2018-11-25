using MongoDB.Bson;

namespace EnglishHubRepository
{
    public class PackageEntity
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string userId { get; set; }
    }
}