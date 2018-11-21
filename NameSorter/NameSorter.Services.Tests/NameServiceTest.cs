using Moq;
using NameSorter.Common;
using NameSorter.Engines.Interfaces;
using NameSorter.Repositories.Interfaces;
using NameSorter.Services.Implementations;
using NameSorter.Services.Interfaces;
using NUnit.Framework;

namespace NameSorter.Services.Tests
{
    [TestFixture]
    public class NameServiceTest
    {
        private readonly INameService _nameService;
        private readonly Mock<INameEngine> _mockNameEngine;
        private readonly Mock<INameRepository> _mockNameRepository;

        public NameServiceTest()
        {
            // bind all interfaces to its implementations
            ObjectFactory.BindAll();

            // mock dependencies
            _mockNameEngine = new Mock<INameEngine>();
            _mockNameRepository = new Mock<INameRepository>();

            // instantiate NameService by injecting mocked dependencies object
            _nameService = new NameService(_mockNameRepository.Object, _mockNameEngine.Object);
        }

        private string[] expected = new string[]
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
        private string[] actual = null;

        [OneTimeSetUp]
        public void TestInitialize()
        {
            // mock Sort function to return the expected
            _mockNameEngine.Setup(e => e.Sort("path")).Returns(expected);
            
            // mock saving the sorted result 
            _mockNameRepository.Setup(e => e.Save(expected));

            // get the actual test result
            actual = _nameService.SortAndSave("path");
        }

        [Test]
        public void SortAndSave_Should_Return_Sort_Result()
        {
            // ensure expected length same with actual length
            Assert.AreEqual(expected.Length, actual.Length);

            for (int i = 0; i < expected.Length; i++)
            {
                // ensure expected same with actual
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [Test]
        public void SortAndSave_Should_Call_Save_Once()
        {
            // ensure Save function must be called once
            _mockNameRepository.Verify(e => e.Save(expected), Times.Once);
        }
    }
}
