using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamo.Helper
{
    public static class HelperMethods
    {
        public static BasicAWSCredentials SetAwsCredentials(string awsId, string awsPassword)
        {
            var creds = new BasicAWSCredentials(awsId, awsPassword);
            return creds;
        }
        
        public static AmazonDynamoDBClient GetDynamoDbClient(BasicAWSCredentials creds, RegionEndpoint awsDynamoDbRegion)
        {
            var client = new AmazonDynamoDBClient(creds, awsDynamoDbRegion);
            return client;
        }
      
        public static DynamoDBContext GetDynamoDbContext(AmazonDynamoDBClient client)
        {
            var context = new DynamoDBContext(client);
            return context;
        }
      
        public static DynamoDBOperationConfig GetDynamoDbOperationConfig(string dynamoDbTable)
        {
            DynamoDBOperationConfig config = new DynamoDBOperationConfig() { OverrideTableName = dynamoDbTable };
            return config;
        }
    }
}
