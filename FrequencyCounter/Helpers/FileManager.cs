using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helpers.FrequencyCounter
{
    internal class FileManager
    {
        private static readonly Encoding ANSIEnconding = Encoding.GetEncoding("Windows-1252");

        internal IEnumerable<string> GetFileLines(string fileName)
        {
            return GetFileLines(fileName, ANSIEnconding);
        }

        internal IEnumerable<string> GetFileLines(string fileName, Encoding encoding)
        {
            CheckIfFileIsValid(fileName);
            return File.ReadLines(fileName, encoding);
        }

        private void CheckIfFileIsValid(string fileName)
        {
            if (fileName == null || fileName == string.Empty)
                throw new ArgumentNullException("File name is required.");
            else if (!File.Exists(fileName))
                throw new FileNotFoundException(string.Concat("Could not find the file ", fileName, "."));
        }
    }
}
