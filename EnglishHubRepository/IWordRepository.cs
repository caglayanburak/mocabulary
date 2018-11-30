using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;

namespace EnglishHubRepository
{
    public interface IWordRepository
    {
        Task<List<WordEntity>> GetAll(string packageId);
        Task<WordEntity> Get(string id);
        Task<bool> Add(WordEntity entity);

        Task<bool> Remove(string id);

        Task<bool> Update(WordEntity entity);

        Task<bool> RemoveAll();

        Task<List<WordEntity>> GetWordsByUserId(string userId, string packageId);

        Task<List<PackageEntity>> GetPackagesByUserId(string userId);

        Task<PackageEntity> AddPackage(PackageEntity entity);
        Task<bool> UpdatePackage(PackageEntity entity);
        Task<bool> RemovePackage(string id);
        Task<bool> FavoritePackage(string id, bool status);
    }
}
