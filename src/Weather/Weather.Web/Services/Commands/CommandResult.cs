namespace Weather.Web.Services.Commands;

public class CommandResult<T>
{
    public T Result { get; }
    public bool IsError { get; }
    
    public CommandResult(bool isError = false)
    {
        IsError = isError;
    }
    
    public CommandResult(T result)
    {
        Result = result;
    }
}