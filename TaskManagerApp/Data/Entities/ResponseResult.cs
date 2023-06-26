using System.Collections.Generic;

namespace TaskManagerApp.Data.Entities
{
    public sealed class ResponseResult<T>
    {
        public ResponseResult() { }

        public ResponseResult(bool success)
        {
            Success = success;
        }

        public ResponseResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public ResponseResult(bool success, List<string> errors)
        {
            Success = success;
            Errors = errors;
        }

        public ResponseResult(bool success, T data)
        {
            Success = success;
            Data = data;
        }

        public ResponseResult(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }


        public bool Success { get; set; } = false;
        public T Data { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> Errors { get; set; }


        public ResponseResult<T> GetResponseResult(object obj)
        {
            return (ResponseResult<T>)obj;
        }
    }
}