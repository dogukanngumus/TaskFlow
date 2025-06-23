using System;
using Microsoft.Extensions.Logging;
using TaskFlow.Application.Dispatchers;

namespace TaskFlow.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
 where TRequest : ICommand<TResponse>
{
     private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName}", requestName);
        var start = DateTime.UtcNow;
        try
        {
            var response = await next();

            var elapsed = DateTime.UtcNow - start;
            _logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms", requestName, elapsed.TotalMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {RequestName}", requestName);
            throw;
        }
    }
}
