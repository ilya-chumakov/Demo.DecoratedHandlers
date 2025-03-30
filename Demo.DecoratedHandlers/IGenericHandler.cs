namespace Demo.DecoratedHandlers;

public interface IGenericHandler<TInput>
{
    Task HandleAsync(TInput input);
}