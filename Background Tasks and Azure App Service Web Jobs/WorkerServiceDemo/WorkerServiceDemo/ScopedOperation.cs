namespace WorkerServiceDemo
{
    public class ScopedOperation : IScopedOperation
    {
        public string OperationId => Guid.NewGuid().ToString();
    }
}
