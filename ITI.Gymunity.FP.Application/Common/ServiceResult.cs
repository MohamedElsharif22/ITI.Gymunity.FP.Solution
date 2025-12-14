using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.Gymunity.FP.Application.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                ErrorMessage = null
            };
        }

        public static ServiceResult<T> Failure(string errorMessage)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Data = default,
                ErrorMessage = errorMessage
            };
        }
    }
}
