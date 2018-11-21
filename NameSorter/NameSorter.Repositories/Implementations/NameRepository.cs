using NameSorter.Repositories.Interfaces;
using System;
using System.Configuration;
using System.IO;

namespace NameSorter.Repositories.Implementations
{
    public class NameRepository : INameRepository
    {
        public string[] Get(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                // ensure file path not empty
                throw new Exception("File path cannot be empty");
            }
            else if(!File.Exists(path))
            {
                // ensure file exists
                throw new Exception(string.Format("Unable to find file: {0}", path));
            }

            // read all lines from specified file path
            return File.ReadAllLines(path);
        }

        public void Save(string[] names)
        {
            // ensure names not empty
            if (names == null || names.Length == 0)
            {
                throw new Exception("Names cannot be empty");
            }

            // get file name from app.config
            string fileName = ConfigurationManager.AppSettings["SaveFileName"];

            // save into working directory
            string filePath = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), fileName);
            File.WriteAllLines(filePath, names);
        }
    }
}
