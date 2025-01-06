using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.DataTransferObjects
{
    public class ResetPasswordRequestDto
    {
        public string EmailAddress { get; set; }
        public string NewPassword { get; set; }
    }
}
