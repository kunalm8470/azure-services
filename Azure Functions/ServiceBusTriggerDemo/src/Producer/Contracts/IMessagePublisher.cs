namespace Producer.Contracts
{
    public interface IMessagePublisher
    {
        public Task PublishMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}
