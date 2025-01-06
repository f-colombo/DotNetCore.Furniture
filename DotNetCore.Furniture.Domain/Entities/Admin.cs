using DotNetCore.Furniture.Domain.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Entities
{
    public class Admin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AdminId { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string AdminLoginId { get; set; } = "A86478927";
        public bool isActivated { get; set; } = false;
        public UserRole Role { get; set; } // Use UserRole enum for Role property
    }
}
