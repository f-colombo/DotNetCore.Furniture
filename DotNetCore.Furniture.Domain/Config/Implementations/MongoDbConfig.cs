using DotNetCore.Furniture.Domain.Config.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Config.Implementations
{
    public partial class MongoDbConfig : IMongoDbConfig
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public MongoDbConfig(string databaseName, string connectionString)
        {
            DatabaseName = databaseName;
            ConnectionString = connectionString;
        }
    }
}
