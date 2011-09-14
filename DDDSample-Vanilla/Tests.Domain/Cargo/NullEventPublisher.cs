namespace DDDSample.Domain.Tests.Cargo
{
    /// <summary>
    /// Implementation of <see cref="IEventPublisher"/> for unit tests.
    /// </summary>
    public class NullEventPublisher : IEventPublisher
    {
        public void Raise<T>(T eventArgs)
        {
        }
    }
}