namespace DDDSample.Domain
{
   /// <summary>
   /// Handles events of class <typeparamref name="T"/>.
   /// </summary>
   /// <typeparam name="T">Type of event.</typeparam>
   public interface IEventHandler<in T>
   {
      /// <summary>
      /// Handles the event.
      /// </summary>
      /// <param name="event">Event object.</param>
      void Handle(T @event);
   }
}