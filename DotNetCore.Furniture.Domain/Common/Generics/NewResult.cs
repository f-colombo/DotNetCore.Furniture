using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Furniture.Domain.Common.Generics
{
    public class NewResult
    {
        public string ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
    }

    public class NewLoginResult
    {
        public string ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
        public string Token { get; set; }
    }

    public class NewLoginResult<T> : NewLoginResult
    {
        public T ResponseDetails { get; set; }
        public string Token { get; set; } // JWT token property

        public static NewLoginResult<T> Success(T instance, string token, string message = "successful")
        {
            return new NewLoginResult<T>
            {
                ResponseCode = "00",
                ResponseDetails = instance,
                ResponseMsg = message,
                Token = token // Set the JWT token
            };
        }

        public static NewLoginResult<T> Failed(T instance, string message = "BadRequest")
        {
            return new NewLoginResult<T>
            {
                ResponseCode = "99",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }

        public static NewLoginResult<T> Error(T instance, string message = "An error occured while processing your request")
        {
            return new NewLoginResult<T>
            {
                ResponseCode = "55",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }
    }

    public class NewResult<T> : NewResult
    {
        public T ResponseDetails { get; set; }

        public static NewResult<T> Success(T instance, string message = "successful")
        {
            return new NewResult<T>
            {
                ResponseCode = "00",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }

        public static NewResult<T> Failed(T instance, string message = "BadRequest")
        {
            return new NewResult<T>
            {
                ResponseCode = "99",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }
        public static NewResult<T> Unauthorized(T instance, string message = "Unauthorized")
        {
            return new NewResult<T>
            {
                ResponseCode = "41",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }

        public static NewResult<T> RestrictedAccess(T instance, string message = "Unauthorized access")
        {
            return new()
            {
                ResponseCode = "40",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }

        public static NewResult<T> InternalServerError(T instance, string message = "Internal Server Error")
        {
            return new()
            {
                ResponseCode = "55",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }
        public static NewResult<T> SessionExpired(T instance, string message = "Session Expired")
        {
            return new()
            {
                ResponseCode = "41",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }

        public static NewResult<T> Error(T instance, string message = "An error occured while processing your request")
        {
            return new NewResult<T>
            {
                ResponseCode = "55",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }
        public static NewResult<T> Duplicate(T instance, string message = "Duplicate request")
        {
            return new NewResult<T>
            {
                ResponseCode = "77",
                ResponseDetails = instance,
                ResponseMsg = message,
            };
        }
    }
}
