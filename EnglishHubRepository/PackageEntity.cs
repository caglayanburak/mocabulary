using MongoDB.Bson;
using System.Collections.Generic;

namespace EnglishHubRepository
{
    public class PackageEntity
    {
        public ObjectId _id { get; set; }
        public string name { get; set; }
        public string userId { get; set; }
        public bool isFavorite { get; set; }

        public List<WordEntity> words { get; set; }
    }
}