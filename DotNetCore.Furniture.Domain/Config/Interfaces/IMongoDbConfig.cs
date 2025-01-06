using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Config.Interfaces
{
    public partial interface IMongoDbConfig
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
}
