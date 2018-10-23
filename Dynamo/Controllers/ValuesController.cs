using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Dynamo.Helper;
using Dynamo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dynamo.Controllers
{
   
    public class ValuesController : Controller
    {
        private static string dynamoDbTable = string.Empty;
        private IDbClientInitialization _clientAccessor;

        public ValuesController(IOptions<Dictionary<string, string>> appSettings, IDbClientInitialization clientAccessor)
        {
            var vals = appSettings.Value;
            dynamoDbTable = vals["dynamoDbTable"];           
            _clientAccessor = clientAccessor;
        }

        [HttpGet("api/data")]
        public async Task<List<MyModel>> GetAllData(string type, string status)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("Type", ScanOperator.Equal, type));
            conditions.Add(new ScanCondition("Status", ScanOperator.Equal, status));
            var response = await _clientAccessor.GetContext().ScanAsync<MyModel>(conditions, HelperMethods.GetDynamoDbOperationConfig(dynamoDbTable)).GetRemainingAsync();
            return response;
        }

        [HttpPost("api/save")]
        public async Task<IActionResult> SaveData([FromBody] List<MyModel> listData, string input, string name, string type)
        {
            List<MyModel> model = null;
            foreach (var data in listData)
            {
                //populating data here
                await _clientAccessor.GetContext().SaveAsync(data, HelperMethods.GetDynamoDbOperationConfig(dynamoDbTable));
            }

            return Ok();
        }

        
        //New Code below

        //private readonly IDynamoDbManager<MyModel> _dynamoDbManager;
        ////This interface is used to setup dynamo db and connection to aws
        //private static string dynamoDbTable = string.Empty;

        //public ValuesController(IOptions<Dictionary<string, string>> appSettings, IDynamoDbManager<MyModel> dynamoDbManager)
        //{
        //    _dynamoDbManager = dynamoDbManager;
        //    var vals = appSettings.Value;
        //    dynamoDbTable = vals["dynamoDbTable"];
        //}

        //[HttpGet("api/data")]
        //public async Task<IActionResult> GetAllData(string type, string status)
        //{
        //    var conditions = new List<ScanCondition>
        //    {
        //        new ScanCondition("Type", ScanOperator.Equal, type),
        //        new ScanCondition("Status", ScanOperator.Equal, status)
        //    };
        //    var result = await _dynamoDbManager.GetAsync(conditions);
        //    var response = result.Select(_ => _.UpdatedBy.ToLower()).ToList();
        //    return Ok(response);
        //}

        //[HttpPost("api/save")]
        //public async Task<IActionResult> SaveData([FromBody] List<MyModel> listData, string input, string name, string type)
        //{
        //    foreach (var data in listData)
        //    {
        //        //populating data here
        //        await _dynamoDbManager.SaveAsync(data);
        //    }
        //    return Ok();
        //}
    }
}
