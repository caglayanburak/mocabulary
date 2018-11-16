using System;
using Xunit;
using EnglishHubRepository;
using System.Threading.Tasks;

namespace EnglishHubUnitTest
{
    public class EnglishWordRepositoryUnitTest
    {
        private IWordRepository hubRepository;
        private string ConnectionString = "mongodb://localhost:27017/";
        private string Database = "englishhub";
        public EnglishWordRepositoryUnitTest()
        {
            hubRepository = new WordRepository(ConnectionString, Database);
        }

        [Fact]
        public async Task Add_NewWord_CheckHasInserted()
        {
            var result = await hubRepository.Add(new WordEntity
            {
                originalword = "apology",
                description = "özür"
            });
            var newEntity = await hubRepository.Get(result._id.ToString());
            Assert.Equal(result._id, newEntity._id);
        }

        [Fact]
        public async Task Add_NewWord_WithUniqueWordCheck()
        {
            //Given
            var result = await hubRepository.Add(new WordEntity
            {
                originalword = "UniqueWord",
                description = "eşsiz kelime"
            });
            //When
            var wordId = result._id;
            var result2 = await hubRepository.Add(new WordEntity
            {
                originalword = "UniqueWord",
                description = "eşsiz kelime"
            });
            var secondAddedWordId = result2._id;


            //Then

            Assert.Equal(wordId, secondAddedWordId);
        }

        [Fact]
        public async Task Add_NewWord_AtLeastOneFieldIsEmpty_ThrowArgumentNullException()
        {
            var exception = await Record.ExceptionAsync(async () => await hubRepository.Add(new WordEntity
            {
            }));

            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}
