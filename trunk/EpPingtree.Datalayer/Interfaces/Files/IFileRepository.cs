using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Datalayer.Interfaces.Files
{
    public interface IFileRepository
    {
        bool SaveFileContents(string fileContents, string path, string filename);
        bool SaveFileContents(string fileContents, string path, string subDir, string filename);
        void SaveObjectToFile<T>(T obj, string path, string filename) where T : class;

        T RetrieveObjectFromFile<T>(string path, string filename, bool checkExists) where T : class;
        T RetrieveObjectFromFile<T>(string fullFilename) where T : class;

        string CombineParts(string directory, string fileOrDir);
    }
}
