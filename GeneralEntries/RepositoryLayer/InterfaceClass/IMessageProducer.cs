namespace GeneralEntries.RepositoryLayer.InterfaceClass;

public interface IMessageProducer
{
    public void SendingMessage<T> (T message);
}
