using MongoDB.Bson;
using System;

namespace EnglishHubRepository
{
    public class AnswerEntity
    {
        public ObjectId _id { get; set; }
        public string Word { get; set; }

        public bool DidKnow { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Attempt { get; set; }
    }

}