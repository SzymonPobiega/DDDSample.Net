namespace DDDSample.Domain
{
    /// <summary>
    /// Publishes events.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <typeparam name="T">Event type.</typeparam>
        /// <param name="eventArgs">Event.</param>
        void Raise<T>(T eventArgs);
    }
}