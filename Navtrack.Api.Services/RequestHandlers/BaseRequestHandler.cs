using System;
using System.Threading.Tasks;
using Navtrack.Api.Model;

namespace Navtrack.Api.Services.RequestHandlers
{
    public abstract class BaseRequestHandler : IRequestHandler
    {
        public ApiResponseModel ApiResponse { get; }

        protected BaseRequestHandler()
        {
            ApiResponse = new ApiResponseModel();
        }
    }

    public abstract class BaseRequestHandler<TRequestModel> : BaseRequestHandler, IRequestHandler<TRequestModel>
    {
        public virtual Task Authorize(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task Validate(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task Handle(TRequestModel request)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class
        BaseRequestHandler<TRequestModel, TResponseModel> : BaseRequestHandler,
            IRequestHandler<TRequestModel, TResponseModel>
    {
        public virtual Task Authorize(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task Validate(TRequestModel request)
        {
            return Task.CompletedTask;
        }

        public virtual Task<TResponseModel> Handle(TRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}