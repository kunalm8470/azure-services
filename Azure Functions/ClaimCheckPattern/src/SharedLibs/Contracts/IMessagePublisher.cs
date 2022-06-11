namespace SharedLibs.Contracts
{
    public interface IMessagePublisher
    {
        Task PublishMessageAsync<T>(T message, string messageId, string? correlationId = default, string? replyTo = default, CancellationToken cancellationToken = default) where T : class;
    }
}
