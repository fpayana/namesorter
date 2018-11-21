using NameSorter.Common;
using NameSorter.Repositories.Implementations;
using NameSorter.Repositories.Interfaces;
using NUnit.Framework;
using System;
using System.Configuration;
using System.IO;

namespace NameSorter.Repositories.Tests
{
    [TestFixture]
    public class NameRepositoryTest
    {
        private readonly INameRepository _nameRepository;

        public NameRepositoryTest()
        {
            // bind engines and repositories interfaces to its implementations
            ObjectFactory.BindRepositories();

            // instantiate NameRepository
            _nameRepository = new NameRepository();
        }

        [Test]
        public void Get_Null_File_Path_Should_Fail()
        {
            // call Get function with null file path expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameRepository.Get(null));

            // ensure ex message correct
            Assert.AreEqual("File path cannot be empty", ex.Message);
        }

        [Test]
        public void Get_Empty_File_Path_Should_Fail()
        {
            // call Get function with empty file path expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameRepository.Get(string.Empty));

            // ensure ex message correct
            Assert.AreEqual("File path cannot be empty", ex.Message);
        }

        [Test]
        public void Get_Not_Found_Path_Should_Fail()
        {
            // call Get function with not found file path expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameRepository.Get("path"));

            // ensure ex message correct
            Assert.AreEqual("Unable to find file: path", ex.Message);
        }

        [Test]
        public void Get_Should_Return_File_Contents()
        {
            string[] expected = new string[] { "Orson Milka Iddins", "Erna Dorey Battelle", };

            // write expected into text file
            File.WriteAllLines("test.txt", expected);

            // call Get function to read the content of text file
            string[] actual = _nameRepository.Get("test.txt");

            // ensure expected length same with actual length
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                // ensure expected same with actual
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [Test]
        public void Save_Null_Names_Should_Fail()
        {
            // call Save function with null names expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameRepository.Save(null));

            // ensure ex message correct
            Assert.AreEqual("Names cannot be empty", ex.Message);
        }

        [Test]
        public void Save_Empty_Names_Should_Fail()
        {
            // call Save function with empty names expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameRepository.Save(new string[] { }));

            // ensure ex message correct
            Assert.AreEqual("Names cannot be empty", ex.Message);
        }

        [Test]
        public void Save_Should_Save_Into_Current_Directory()
        {
            string[] expected = new string[] { "Orson Milka Iddins", "Erna Dorey Battelle", };

            _nameRepository.Save(expected);

            // file path
            string fileName = ConfigurationManager.AppSettings["SaveFileName"];
            string filePath = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), fileName);

            // read the actual file contents
            string[] actual = File.ReadAllLines(filePath);

            // ensure expected length same with actual length
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                // ensure expected same with actual
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
    }
}
