namespace WaveAction.Domain.Common.Exceptions;


public class EntityFoundException : Exception
{
    public EntityFoundException() { }
    public EntityFoundException(string message) : base(message) { }
}
