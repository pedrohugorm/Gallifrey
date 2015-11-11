using System;

namespace Gallifrey.RestApi.Application.Domain.Model
{
    public abstract class OperationResponse
    {
        public bool IsSuccess { set; get; }
        public int Code { set; get; }
        public string[] Messages { set; get; }
        public Exception Exception { set; get; }

        protected OperationResponse()
        {
            IsSuccess = true;
        }

        protected OperationResponse(Exception exception)
        {
            IsSuccess = false;
            Exception = exception;
        }
    }
}