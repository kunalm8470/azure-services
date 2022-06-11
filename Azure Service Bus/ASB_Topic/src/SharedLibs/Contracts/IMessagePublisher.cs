namespace SharedLibs.Contracts
{
    public interface IMessagePublisher
    {
        Task PublishMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    }
}
