using System;

namespace Gallifrey.RestApi.Application.Domain.Model
{
    public class ResponseContainer<TModel> : OperationResponse, IResponse<TModel>
    {
        public TModel Response { get; set; }

        public ResponseContainer()
        {
        }

        public ResponseContainer(TModel response)
        {
            Response = response;
        }

        public ResponseContainer(Exception exception)
            : base(exception)
        {
        }
    }
}