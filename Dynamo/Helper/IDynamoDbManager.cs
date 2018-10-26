using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamo.Helper
{
    public interface IDynamoDbManager<T> : IDisposable where T : class
    {
        Task<List<T>> GetAsync(IEnumerable<ScanCondition> conditions);

        Task SaveAsync(T item);

        Task DeleteAsync(T item);
    }
}
