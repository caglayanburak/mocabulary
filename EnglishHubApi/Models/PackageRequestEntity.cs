
namespace EnglishHubApi
{
    public class PackageEntityRequest
    {
        public string _id { get; set; }
        public string name { get; set; }
        public bool isFavorite { get; set; }
        public int starCount { get; set; }
    }
}