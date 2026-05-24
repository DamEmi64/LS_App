using FluentResults;
using MediatR;

namespace Base
{
    public class ConnectClient : IConnect
    {
        private readonly IMediator _mediator;

        public ConnectClient(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Send<T1>(T1 data) where T1 : IRequest<Result>
        {
            return await _mediator.Send(data);
        }

        public async Task<Result<T2>> Send<T1, T2>(T1 data) where T1 : IRequest<Result<T2>> where T2 : class
        {
            return await _mediator.Send(data);
        }
    }
}
