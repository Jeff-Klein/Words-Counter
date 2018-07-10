using Helpers.FrequencyCounter;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Counters.FrequencyCounter
{
    public class FrequencyWordsCounter
    {
        private static readonly char[] splitters = { ' ', '\t', '\n', '\r' };
        private double progressPercentage = 0;
        private double ProgressPercentage
        {
            get { return progressPercentage; }
            set
            {
                if (progressPercentage == value)
                    return;

                progressPercentage = value;
                OnProgressChange?.Invoke(progressPercentage);
            }
        }
        public delegate void OnProgressChangeDelegate(double newPercentage);
        public event OnProgressChangeDelegate OnProgressChange;
        public CancellationTokenSource cancelToken;

        public IOrderedEnumerable<KeyValuePair<string, int>> CountWordsFromTextFile(string fileName)
        {
            var fileManager = new FileManager();
            var fileLines = fileManager.GetFileLines(fileName);
            return CountWordsFromList(fileLines);
        }

        public IOrderedEnumerable<KeyValuePair<string, int>> CountWordsFromList(IEnumerable<string> fileLines)
        {
            cancelToken = new CancellationTokenSource();
            var wordsCountList = new ConcurrentDictionary<string, int>();
            var totalNumberOfLines = fileLines.Count();
            var processedLines = 0;

            try
            {               
                Parallel.ForEach(fileLines, (line, state) => 
                {
                    if (cancelToken.Token.IsCancellationRequested)
                        state.Stop();

                    var wordsInLine = line.Split(splitters, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in wordsInLine)
                        wordsCountList.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);

                    Interlocked.Increment(ref processedLines);
                    CalculateProgress(processedLines, totalNumberOfLines);
                });

                return wordsCountList.OrderByDescending(v => v.Value).ThenBy(k => k.Key);
            }
            catch(AggregateException ex)
            {
                var message = "There were one or more errors during the counting process. ";

                if (ex.InnerException != null)
                    message += ex.InnerException.Message;
                
                throw new Exception(string.Concat("There were errors during the counting process. " + ex.InnerException.Message), ex);
            }
        }

        private void CalculateProgress(int linesProcessed, int totalNumberOfLines)
        {
            ProgressPercentage = linesProcessed * 100 / totalNumberOfLines;
        }
    }
}
