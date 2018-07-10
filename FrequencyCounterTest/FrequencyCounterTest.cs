using System.Collections.Generic;
using System.Linq;
using Counters.FrequencyCounter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrequencyCounterTest
{
    [TestClass]
    public class FrequencyCounterTest
    {
        [TestMethod]
        public void CountWordsTest()
        {

            List<KeyValuePair<string, int>> expectedValues = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("Adam", 2),
                new KeyValuePair<string, int>("Seth", 2),
                new KeyValuePair<string, int>("1:1", 1),
                new KeyValuePair<string, int>("1:2", 1),
                new KeyValuePair<string, int>("Cainan", 1),
                new KeyValuePair<string, int>("Enos", 1),
                new KeyValuePair<string, int>("Iared", 1)
            };

            FrequencyWordsCounter wordsCounter = new FrequencyWordsCounter();
            var wordsList = wordsCounter.CountWordsFromTextFile("../../SampleFiles/Sample1.txt");

            if (!wordsList.ToList().SequenceEqual(expectedValues.ToList()))
                Assert.Fail();

        }

        [TestMethod] 
        public void CountWordsWithSplittersTest()
        {

            List<KeyValuePair<string, int>> expectedValues = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("test4", 4),
                new KeyValuePair<string, int>("test3", 3),
                new KeyValuePair<string, int>("test2", 2),
                new KeyValuePair<string, int>("test1", 1)
            };

            FrequencyWordsCounter wordsCounter = new FrequencyWordsCounter();
            var wordsList = wordsCounter.CountWordsFromTextFile("../../SampleFiles/Sample2.txt");

            if (!wordsList.ToList().SequenceEqual(expectedValues.ToList()))
                Assert.Fail();

        }

        [TestMethod]
        public void CountNumbersTest()
        {

            List<KeyValuePair<string, int>> expectedValues = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>("1", 7),
                new KeyValuePair<string, int>("1,00", 1),
                new KeyValuePair<string, int>("1.000", 1),
                new KeyValuePair<string, int>("1.000,00", 1),
                new KeyValuePair<string, int>("10", 1),
                new KeyValuePair<string, int>("100", 1),
                new KeyValuePair<string, int>("11", 1)
            };

            FrequencyWordsCounter wordsCounter = new FrequencyWordsCounter();
            var wordsList = wordsCounter.CountWordsFromTextFile("../../SampleFiles/Sample3.txt");

            if (!wordsList.ToList().SequenceEqual(expectedValues.ToList()))
                Assert.Fail();

        }

        [TestMethod]
        public void CountSpecialCharsTest()
        {

            List<KeyValuePair<string, int>> expectedValues = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>(",", 2),
                new KeyValuePair<string, int>(";:", 2),
                new KeyValuePair<string, int>("~^´`!@#$%¨&", 2),
                new KeyValuePair<string, int>("ªº°§", 2),
                 new KeyValuePair<string, int>(".", 1),
                new KeyValuePair<string, int>("ãâçéè", 1)                
            };

            FrequencyWordsCounter wordsCounter = new FrequencyWordsCounter();
            var wordsList = wordsCounter.CountWordsFromTextFile("../../SampleFiles/Sample4.txt");

            if (!wordsList.ToList().SequenceEqual(expectedValues.ToList()))
                Assert.Fail();

        }
    }
}
