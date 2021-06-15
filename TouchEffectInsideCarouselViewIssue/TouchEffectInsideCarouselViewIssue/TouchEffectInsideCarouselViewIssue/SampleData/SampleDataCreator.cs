using System;
using System.Collections.Generic;
using System.Text;

namespace TouchEffectInsideCarouselViewIssue.SampleData
{
    public static class SampleDataCreator
    {
        private static readonly Random Rand = new Random();

        public static IList<SampleModel> CreateSampleData(int amount)
        {
            var result = new List<SampleModel>();
            for (var i = 0; i < amount; i++)
            {
                var imageWidth = Rand.Next(200, 300);
                var imageHeight = Rand.Next(200, 300);
                var imageUrl = $"https://picsum.photos/{imageWidth}/{imageHeight}";
                var title = LoremIpsum(2, 4, 1, 1, 1);
                var description = LoremIpsum(2, 50, 1, 10, 1);
                result.Add(new SampleModel {Title = title, LongDescription = description, ImageUrl = imageUrl});
            }

            return result;
        }

        private static string LoremIpsum(int minWords, int maxWords,
            int minSentences, int maxSentences,
            int numParagraphs)
        {
            var words = new[]
            {
                "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
                "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
                "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
            };

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                               + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0)
                        {
                            result.Append(" ");
                        }

                        result.Append(words[rand.Next(words.Length)]);
                    }

                    result.Append(". ");
                }
            }

            return result.ToString();
        }
    }
}