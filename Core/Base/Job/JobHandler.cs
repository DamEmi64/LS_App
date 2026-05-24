using FluentResults;
using MediatR;

namespace Base;

public abstract class JobHandler<T> : IRequestHandler<T, Result> where T : IJob
{
    private readonly IJobContext _jobContext;

    public JobHandler(IJobContext jobContext)
    {
        _jobContext = jobContext;
    }

    public abstract Task Execute(T request);

    public async Task<Result> Handle(T request, CancellationToken cancellationToken)
    {
        try
        {
            await _jobContext.OnStart();
            await Execute(request);
        }
        finally
        {
            await _jobContext.OnComplete();
        }

        return Result.Ok();
    }

    public Task LogError(string log) => _jobContext.AddError(log);
    public Task Log(string log) => _jobContext.AddLog(log);
}
