using Moq;
using NameSorter.Common;
using NameSorter.Engines.Implementations;
using NameSorter.Engines.Interfaces;
using NameSorter.Entities;
using NameSorter.Repositories.Interfaces;
using NUnit.Framework;
using System;

namespace NameSorter.Engines.Tests
{
    [TestFixture]
    public class NameEngineTest
    {
        private readonly INameEngine _nameEngine;
        private readonly Mock<INameRepository> _mockNameRepository;

        public NameEngineTest()
        {
            // bind engines and repositories interfaces to its implementations
            ObjectFactory.BindEngines();
            ObjectFactory.BindRepositories();

            // mock dependencies
            _mockNameRepository = new Mock<INameRepository>();

            // instantiate NameEngine by injecting mocked dependencies object
            _nameEngine = new NameEngine(_mockNameRepository.Object);
        }

        [Test]
        public void Sort_Null_File_Contents_Should_Fail()
        {
            // mock file contents as null
            _mockNameRepository.Setup(e => e.Get("path")).Returns((string[]) null);

            // call Sort function with null file contents expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameEngine.Sort("path"));

            // ensure ex message correct
            Assert.AreEqual("File contents cannot be empty", ex.Message);
        }

        [Test]
        public void Sort_Empty_File_Contents_Should_Fail()
        {
            // mock file contents as empty
            _mockNameRepository.Setup(e => e.Get("path")).Returns(new string[] { });

            // call Sort function with empty file contents expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameEngine.Sort("path"));

            // ensure ex message correct
            Assert.AreEqual("File contents cannot be empty", ex.Message);
        }


        [Test]
        public void Sort_Should_Allow_Empty_Line()
        {
            string[] fileContents = new string[]
            {
                "Orson Milka Iddins",
                "",
                "Erna Dorey Battelle"
            };
            string[] expected = new string[]
            {
                "Erna Dorey Battelle",
                "Orson Milka Iddins"
            };

            // mock file contents
            _mockNameRepository.Setup(e => e.Get("path")).Returns(fileContents);

            // call Sort function and get the actual result
            string[] actual = _nameEngine.Sort("path");

            // ensure expected length same with actual length
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                // ensure expected same with actual
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [Test]
        public void Sort_Should_Sort_By_LastName_Then_GivenName()
        {
            string[] fileContents = new string[]
            {
                "Orson Milka Iddins",
                "Erna Dorey Battelle",
                "Flori Chaunce Franzel",
                "Odetta Sue Kaspar",
                "Roy Ketti Kopfen",
                "Madel Bordie Mapplebeck",
                "Selle Bellison",
                "Leonerd Adda Mitchell Monaghan",
                "Debra Micheli",
                "Hailey Avie Annakin"
            };
            string[] expected = new string[] 
            {
                "Hailey Avie Annakin",
                "Erna Dorey Battelle",
                "Selle Bellison",
                "Flori Chaunce Franzel",
                "Orson Milka Iddins",
                "Odetta Sue Kaspar",
                "Roy Ketti Kopfen",
                "Madel Bordie Mapplebeck",
                "Debra Micheli",
                "Leonerd Adda Mitchell Monaghan"
            };

            // mock file contents
            _mockNameRepository.Setup(e => e.Get("path")).Returns(fileContents);

            // call Sort function and get the actual result
            string[] actual = _nameEngine.Sort("path");

            // ensure expected length same with actual length
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                // ensure expected same with actual
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [Test]
        public void GetName_Null_Text_Should_Return_Null()
        {
            // call GetName function with null text
            Name actual = _nameEngine.GetName(null);

            // ensure actual is null
            Assert.AreEqual(null, actual);
        }

        [Test]
        public void GetName_Empty_Text_Should_Return_Null()
        {
            // call GetName function with empty text
            Name actual = _nameEngine.GetName(string.Empty);

            // ensure actual is null
            Assert.AreEqual(null, actual);
        }

        [Test]
        public void GetName_One_Text_Part_Should_Fail()
        {
            // call GetName function with 1 text part expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameEngine.GetName("Orson"));

            // ensure ex message correct
            Assert.AreEqual("A name must have at least 1 given name and a last name: Orson", ex.Message);
        }

        [Test]
        public void GetName_Five_Text_Parts_Should_Fail()
        {
            // call GetName function with 5 text parts expect an exception thrown
            Exception ex = Assert.Throws<Exception>(() => _nameEngine.GetName("Orson Leonerd Adda Mitchell Monaghan"));

            // ensure ex message correct
            Assert.AreEqual("A name may have up to 3 given names and a last name: Orson Leonerd Adda Mitchell Monaghan", ex.Message);
        }

        [Test]
        public void GetName_Should_Return_Name()
        {
            // call GetName function should return Name object
            Name name = _nameEngine.GetName("Leonerd Adda Mitchell Monaghan");

            // ensure name not null
            Assert.IsNotNull(name);

            // ensure given name correct
            Assert.AreEqual("Leonerd Adda Mitchell", name.GivenName);

            // ensure last name correct
            Assert.AreEqual("Monaghan", name.LastName);
        }
    }
}
