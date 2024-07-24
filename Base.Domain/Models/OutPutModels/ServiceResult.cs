using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.OutPutModels
{
    public class ServiceResult
    {
        /// <summary>
        /// Messages
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();

        /// <summary>
        /// Result Status
        /// </summary>
        public bool IsSuccess { get; set; }
        public bool IsFailed { get; set; }


        /// <summary>
        /// Error With One Message
        /// </summary>
        /// <param name="message">ErrorMessage  that will be shown</param>
        public ServiceResult(string message)
        {
            IsSuccess = false;
            IsFailed = true;
            Messages.Add(message);
        }

        /// <summary>
        /// Custom Success With One Message
        /// </summary>
        /// <param name="message">Custom Message that will be shown</param>
        public ServiceResult(string message, bool isSuccess)
        {
            IsSuccess = isSuccess;
            IsFailed = !isSuccess;
            Messages.Add(message);
        }

        /// <summary>
        /// Error WithMany Messages
        /// </summary>
        /// <param name="errorMessages">ErrorMessages string List that will be shown</param>
        public ServiceResult(List<string> errorMessages)
        {
            IsSuccess = false;
            IsFailed = true;
            Messages = errorMessages;
        }
        /// <summary>
		/// Success With Default Message And No ResultObject
		/// </summary>
		public ServiceResult()
        {
            IsSuccess = true;
            IsFailed = false;
            Messages.Add("عملیات با موفقیت انجام شد!");
        }

        /// <summary>
        /// Mapping Service Results To EachOther, Maybe a srvice result with an object to without an object
        /// </summary>
        /// <param name="serviceResult">Service Result Maybe with an object to map</param>
        public ServiceResult(ServiceResult serviceResult)
        {
            IsSuccess = serviceResult.IsSuccess;
            IsFailed = serviceResult.IsFailed;
            Messages = serviceResult.Messages;
        }
    }




    public class ServiceResult<T> : ServiceResult
    {
        /// <summary>
        /// Result Object
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Success With Result Object and Optional Message
        /// </summary>
        /// <param name="result">Result Object</param>
        /// <param name="successMessage">Otional Message To Show User</param>
        public ServiceResult(T result, string successMessage = null)
        {
            IsSuccess = true;
            IsFailed = false;
            Result = result;
            if (successMessage != null)
                Messages.Add(successMessage);
        }

        /// <summary>
        /// Map ServiceResult With Null Result Object
        /// </summary>
        /// <param name="serviceResult">Service Result You want TO Map</param>
        public ServiceResult(ServiceResult serviceResult)
        {
            IsSuccess = serviceResult.IsSuccess;
            IsFailed = serviceResult.IsFailed;
            Messages = serviceResult.Messages;
        }

        public ServiceResult(List<string> messages) : base(messages) { }
        public ServiceResult(string message) : base(message) { }

    }
}
