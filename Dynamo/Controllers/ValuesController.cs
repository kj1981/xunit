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
        private readonly IDynamoDbManager<MyModel> _dynamoDbManager;

        public ValuesController(IOptions<Dictionary<string, string>> appSettings, IDynamoDbManager<MyModel> dynamoDbManager)
        {
            var vals = appSettings.Value;
            dynamoDbTable = vals["dynamoDbTable"];
            _dynamoDbManager = dynamoDbManager;
        }

        [HttpGet("api/data")]
        public async Task<IActionResult> GetAllData(string status, string role)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("status", ScanOperator.Contains, status));
            conditions.Add(new ScanCondition("role", ScanOperator.Contains, role));

            List<MyModel> model = new List<MyModel>();
            model = await _dynamoDbManager.GetAsync(conditions);            

            return Ok(model);
        }

        [HttpPost("api/save")]
        public async Task<IActionResult> SaveData([FromBody] List<MyModel> listData, string type)
        {
            List<MyModel> model = new List<MyModel>();
            foreach (var data in listData)
            {
                //populating data here and saving
                 await _dynamoDbManager.SaveAsync(data);
            }
            return Ok();
        }

        [HttpDelete("api/delete")]
        public async Task<IActionResult> DeleteData(string id)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("id", ScanOperator.Equal, id));

            MyModel model = new MyModel();
            ///populating model here
            await _dynamoDbManager.DeleteAsync(model);
            return Ok();
        }

    }
}
