using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EnglishHubRepository
{
    public interface IAnswerRepository
    {
         Task<List<AnswerEntity>> GetAll();
         Task<AnswerEntity> Get(string id);
         Task<AnswerEntity> Add(AnswerEntity entity);
         Task<List<AnswerEntity>> AddMany(List<AnswerEntity> entities);

         Task<bool> Remove(string id);

         Task<bool> Update(AnswerEntity entity);

         Task<bool> RemoveAll();

         Task<List<AnswerEntity>> GetResults();
    }
}
