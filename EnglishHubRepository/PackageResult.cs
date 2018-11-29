using MongoDB.Bson;
using System;

namespace EnglishHubRepository
{
    public class PackageResult
    {
       public PackageEntity package { get; set; }
       public int wordCount { get; set; }
    }

}