using Amazon.DynamoDBv2.DataModel;
using Dynamo.Controllers;
using Dynamo.Helper;
using Dynamo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ValuesControllerTests
{
    public class ValuesControllerTests
    {
        private Mock<IDynamoDbManager<MyModel>> _dbManager;
        private ValuesController _valueController;

        public ValuesControllerTests()
        {
            var mockRepository = new MockRepository(MockBehavior.Loose);
            _dbManager = mockRepository.Create<IDynamoDbManager<MyModel>>();
            var options = new OptionsWrapper<Dictionary<string, string>>(new Dictionary<string, string>()
            {
                {"dynamoDbTable", nameof(MyModel) }
            });
            _valueController = new ValuesController(options, _dbManager.Object);
        }

        [Fact]
        public async Task GetAllData_Test()
        {
            var searchResult = new List<MyModel>()
            {
                new MyModel(){ }
            };

            _dbManager
             .Setup(_ => _.GetAsync(It.IsAny<List<ScanCondition>>()))
             .ReturnsAsync(searchResult);

            var result = _valueController.GetAllData("new", "admin");
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task SaveData_Test()
        {
            var searchResult = new List<MyModel>()
            {
                new MyModel(){ }
            };

            _dbManager
             .Setup(_ => _.GetAsync(It.IsAny<List<ScanCondition>>()))
             .ReturnsAsync(searchResult);

            var dataToSave = new List<MyModel>()
            {
                new MyModel(){},
                new MyModel(){}
            };

            var result = _valueController.SaveData(dataToSave, "new");
            Assert.IsType<OkResult>(result.Result);
        }

        [Fact]
        public async Task DeleteData_Test()
        {
            var searchResult = new List<MyModel>()
            {
                new MyModel(){ }
            };

            _dbManager
             .Setup(_ => _.GetAsync(It.IsAny<List<ScanCondition>>()))
             .ReturnsAsync(searchResult);

            var result = await _valueController.DeleteData("id123");
            Assert.IsType<OkResult>(result);
        }
    }
}
