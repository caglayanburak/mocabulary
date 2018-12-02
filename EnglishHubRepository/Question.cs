using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;

namespace EnglishHubRepository.Models
{
    public class Question
    {
        public Question()
        {
            Options = new List<string>();
        }
        public string QuestionWord { get; set; }
        public List<string> Options { get; set; }
        public string Answer { get; set; }

        public void PrepareOptions(List<string> options)
        {
            int[] numbers = new int[3];
            var counter = 0;
            Random random = new Random();
            var answerRandomNumber = random.Next(0, 3);
            do
            {
                random = new Random();
                var randomNumber = random.Next(0, options.Count);

                if (Array.IndexOf(numbers, randomNumber) == -1)
                {
                    if (answerRandomNumber == counter)
                    {
                        Options.Add(Answer);
                        numbers[counter] = -99;
                        counter++;
                        continue;
                    }
                    numbers[counter] = randomNumber;
                    Options.Add(options[randomNumber]);
                    counter++;
                }

            } while (counter < 3);
        }

        public List<string> GetRandomWords(List<string> words, int totalWord = 4)
        {
            int[] numbers = new int[totalWord];
            var counter = 0;
            Random random;
            List<string> qWords = new List<string>();

            do
            {
                random = new Random();
                var randomNumber = random.Next(0, words.Count);
                if (Array.IndexOf(numbers, randomNumber) == -1)
                {
                    numbers[counter] = randomNumber;
                    qWords.Add(words[randomNumber]);
                    Console.WriteLine(qWords[counter] + "-" + counter);
                    counter++;
                }

            } while (counter < totalWord);

            return qWords;
        }

    }
}