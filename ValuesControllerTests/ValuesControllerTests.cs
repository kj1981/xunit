using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dynamo.Helper;
using Dynamo.Model;
using Dynamo.Controllers;

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
             .Setup(_ => _.GetAsync(It.Is<List<ScanCondition>>(list => list.Count == 2)))
             .ReturnsAsync(searchResult);

            var result = await _valueController.GetAllData("new", "admin");

            var okResult = result as OkObjectResult;

            Assert.NotNull(okResult);
        }
    }
}
