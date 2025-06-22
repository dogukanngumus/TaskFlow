namespace TaskFlow.Application.Behaviours;

public interface IPipelineBehavior<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
}
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();