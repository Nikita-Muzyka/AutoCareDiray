using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AutoCareDiray.Shared.Service.ResultService
{
    public class Result
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }


        public Result()
        {
        }
        protected Result(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }



        public static Result SuccessCreate() => new Result(true, string.Empty);

        public static Result ErrorCreate(string error) => new Result(false, error);
    }
    public class Result<T> : Result
    {
        public T? Data { get; set; }

        public Result() { }
        private Result(bool success, string errorMessage, T data) : base(success, errorMessage)
        {
            Data = data;
        }
        public static Result<T> SuccessCreate(T data) => new Result<T>(true, string.Empty, data);
    }
}

