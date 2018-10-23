using Amazon.DynamoDBv2.DataModel;
using Dynamo.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamo.Helper
{
    public class DbClientInitialization: IDbClientInitialization
    {
        private readonly DbClientSettings settings;
        private DynamoDBContext _awsContext;

        public DbClientInitialization(IOptions<DbClientSettings> options)
        {
            settings = options?.Value;
        }

        public DynamoDBContext GetContext()
        {
            //Check is context already exists. If not create a new one.
            if (_awsContext != null)
            {
                return _awsContext;
            }
            else
            {
                var creds = HelperMethods.SetAwsCredentials(settings.Id, settings.Password);
                var dynamoClient = HelperMethods.GetDynamoDbClient(creds, settings.Region);
                _awsContext = HelperMethods.GetDynamoDbContext(dynamoClient);

                return _awsContext;
            }

        }
    }
}
