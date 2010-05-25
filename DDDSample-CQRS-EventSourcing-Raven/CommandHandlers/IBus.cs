namespace DDDSample.CommandHandlers
{
   public interface IBus
   {
      void Send<T>(T command);
   }
}