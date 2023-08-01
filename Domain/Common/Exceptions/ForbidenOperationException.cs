namespace WaveAction.Domain.Common.Exceptions;

public class ForbidenOperationException : Exception
{
    public ForbidenOperationException() { }
    public ForbidenOperationException(string message) : base(message) { }
}
