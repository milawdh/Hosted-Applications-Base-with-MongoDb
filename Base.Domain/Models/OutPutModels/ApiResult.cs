using MongoDB.Driver;

namespace Base.Domain.Models.OutPutModels
{
    public class ApiResult
    {
        public bool Success { get; set; }

        /// <summary>
        /// Api Result StatusCode
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Messages
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// Mapping Service Result to api result
        /// </summary>
        /// <param name="serviceResult"></param>
        public ApiResult(ServiceResult serviceResult)
        {
            Messages = serviceResult.Messages;
            Success = serviceResult.IsSuccess;

            if (serviceResult.IsFailed)
                StatusCode = 400;

            if (serviceResult.IsSuccess)
                StatusCode = 200;
        }

        /// <summary>
        /// Api Response With One Error
        /// </summary>
        /// <param name="error">Error Message</param>
        public ApiResult(string error)
        {
            StatusCode = 400;
            Success = false;
            if (error != null)
                Messages.Add(error);
        }

        /// <summary>
        /// Api Response With Many Errors
        /// </summary>
        /// <param name="errors">Error Messages</param>
        public ApiResult(List<string> errors)
        {
            StatusCode = 400;
            Success = false;
            if (errors != null)
                Messages = errors;
        }

        /// <summary>
        /// Initialize Constructor
        /// </summary>
        public ApiResult()
        {
            StatusCode = 200;
            Success = true;
        }

    }



    public class ApiResult<T> : ApiResult
    {

        /// <summary>
        /// Result Object
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Api Result With Object Direct From Service
        /// </summary>
        /// <param name="serviceResult">Result From Service</param>
        public ApiResult(ServiceResult<T> serviceResult) : base(serviceResult)
        {
            Success = serviceResult.IsSuccess;
            Result = serviceResult.Result;
        }

        /// <summary>
        /// Api Result With Object Direct From Service With Optional One Message
        /// </summary>
        /// <param name="result">Result Object</param>
        /// <param name="message">Optional Message</param>
        public ApiResult(T result)
        {
            Success = true;
            Result = result;
        }

        public ApiResult(string message) : base(message) { }
        public ApiResult(List<string> messages) : base(messages) { }
    }
}
