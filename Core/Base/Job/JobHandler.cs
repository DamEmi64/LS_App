using FluentResults;
using MediatR;

namespace Base;

/// <summary>
///     Base MediatR handler for jobs that manages job lifecycle callbacks and logging helpers.
/// </summary>
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
        catch (Exception ex)
        {
            await LogError(ex.Message);
            throw;
        }
        finally
        {
            await _jobContext.OnComplete();
        }

        return Result.Ok();
    }

    public Task LogError(string log) => _jobContext.AddError(log);
    public Task Log(string log) => _jobContext.AddLog(log);
    public void PassData<T2>(T2 data) => _jobContext.PassData(data);
    public T2? GetData<T2>() => _jobContext.GetData<T2>();
}
