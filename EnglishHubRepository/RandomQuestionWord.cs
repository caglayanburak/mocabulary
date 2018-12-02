using MongoDB.Bson;
using System.Collections.Generic;

namespace EnglishHubRepository
{
    public class RandomQuestionWord
    {
        public ObjectId _id { get; set; }
        public ObjectId wordId { get; set; }
        public ObjectId packageId { get; set; }
    }
}