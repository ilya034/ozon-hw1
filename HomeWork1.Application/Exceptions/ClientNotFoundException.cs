namespace HomeWork1.Application.Exceptions;

public sealed class ClientNotFoundException : Exception
{
    public ClientNotFoundException(long clientId)
        : base($"Client with id {clientId} was not found in order log.")
    {
        ClientId = clientId;
    }

    public long ClientId { get; }
}