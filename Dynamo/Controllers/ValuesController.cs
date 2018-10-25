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

            var result = await _dynamoDbManager.GetAsync(conditions);
            model = (from r in result
                     select r).ToList();

            return Ok(model);
        }

        [HttpPost("api/save")]
        public async Task<IActionResult> SaveData([FromBody] List<MyModel> listData, string input, string name, string type)
        {
            List<MyModel> model = new List<MyModel>();
            foreach (var data in listData)
            {
                //populating data here
                //await await _dynamoDbManager.SaveAsync(model);
            }

            return Ok();
        }       
       
    }
}
