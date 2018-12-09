using EnglishHubRepository;
namespace EnglishHubApi
{
    public class WordRequestEntity
    {
        public string lexialCategory { get; set; }
        public string definition { get; set; }

        public string id { get; set; }
        public string word { get; set; }
        public string description { get; set; }
        public string userId { get; set; }
        public string packageId { get; set; }
        public string ownSentence { get; set; }
        public string synonym { get; set; }
        public PackageEntityRequest packageEntity { get; set; }
    }
}