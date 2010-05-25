namespace DDDSample.CommandHandlers
{
   public interface ICommandHandler<T>
   {
      void Handle(T command);
   }
}