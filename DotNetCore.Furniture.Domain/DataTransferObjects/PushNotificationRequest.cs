using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.DataTransferObjects
{
    public class PushNotificationRequest
    {
        public string UserId { get; set; }
        public string body { get; set; }
        // public string DeviceToken { get; set; }
    }
}
