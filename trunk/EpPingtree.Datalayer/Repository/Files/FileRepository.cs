using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using EpPingtree.Datalayer.Interfaces.Files;
using log4net;

namespace EpPingtree.Datalayer.Repository.Files
{
    public class FileRepository: BaseRepository, IFileRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public bool SaveFileContents(string fileContents, string path, string subDir, string filename)
        {
            log4net.Config.XmlConfigurator.Configure(); 


            try
            {
                EnsurePathExists(path);

                if (!string.IsNullOrEmpty(subDir))
                {
                    path = Path.Combine(path, subDir);
                    EnsurePathExists(path);
                }

                //Remove the invalid file characters \/:?*"<>|
                filename = filename.Replace("\\", "").Replace("/", "").Replace(":", "").
                    Replace("?", "").Replace("*", "").Replace("\"", "").Replace("<", "").
                    Replace(">", "").Replace("|", "");

                string fullFileName = Path.Combine(path, filename);

                using (StreamWriter writer = new StreamWriter(fullFileName))
                {
                    writer.Write(fileContents);
                }

                return true;
            }
            catch (Exception e)
            {
                string errorMsg = string.Format("Error saving file contents to {0}/{1}/{2}", path, subDir, filename);
                Log.Error(errorMsg, e);
            }

            //Save failed
            return false;
        }

        public bool SaveFileContents(string fileContents, string path, string filename)
        {
            return SaveFileContents(fileContents, path, "", filename);
        }

        private void EnsurePathExists(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public void SaveObjectToFile<T>(T obj, string path, string filename) where T : class
        {
            EnsurePathExists(path);

            string fullFileName = Path.Combine(path, filename);

            BinaryFormatter serializer = new BinaryFormatter();

            using (FileStream stream = new FileStream(fullFileName, FileMode.CreateNew))
            {
                serializer.Serialize(stream, obj);
                stream.Flush();
            }
        }

        public T RetrieveObjectFromFile<T>(string path, string filename, bool checkExistsBeforeOpen) where T : class
        {
            string fullFilename = Path.Combine(path, filename);

            if (checkExistsBeforeOpen)
            {
                //Caller is aware that this file may not exist
                //If file doesn't exist, return null
                if (!File.Exists(fullFilename))
                    return null;
            }

            return RetrieveObjectFromFile<T>(fullFilename);

        }

        public T RetrieveObjectFromFile<T>(string fullFilename) where T : class
        {
            BinaryFormatter serializer = new BinaryFormatter();

            //Open for read so can handle concurrency
            using (FileStream readStream = File.OpenRead(fullFilename))
            {
                T obj = (T)serializer.Deserialize(readStream);
                return obj;
            }
        }

        public string CombineParts(string directory, string fileOrDir)
        {
            return Path.Combine(directory, fileOrDir);
        }
    }
}
