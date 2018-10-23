using Amazon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamo.Model
{
    public class DbClientSettings
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public RegionEndpoint Region { get; set; }
    }
}
