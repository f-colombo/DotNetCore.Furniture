using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Entities
{
    public class MyBankLog
    {
        public string ServiceName { get; set; }
        public string Endpoint { get; set; }
        public string UserId { get; set; }
        public string ChannelId { get; set; }
        public string RequestDate { get; set; }
        public string RequestDetails { get; set; }
        public string ResponseDate { get; set; }
        public string UserToken { get; set; }
        public string Response { get; set; } = "Failed";
        public string ResponseDetails { get; set; }
        public string AdditionalInformation { get; set; }
        public string Amount { get; set; }
    }
}
