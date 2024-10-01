namespace GeneralEntries.RepositoryLayer.InterfaceClass
{
    public interface IRabbitMqMessageService
    {
        Task PublishMessage(string queueConnStr, string queueName, List<Tuple<Dictionary<string, object>, string>> messages);
        Task PublishMessage(string queueConnStr, string queueName, Dictionary<string, object> header, string message);
    }
}
