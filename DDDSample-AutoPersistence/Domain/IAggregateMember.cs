namespace DDDSample.Domain
{
   public interface IAggregateRoot<T> : IAggregateRoot, IEntity<T>
      where T : IAggregateRoot
   {      
   }

   public interface IAggregateRoot : IAggregateMember
   {
   }

   public interface IEntity<T> : IAggregateMember<T>, IEntity
      where T : IAggregateRoot
   {
   }

   public interface IEntity : IAggregateMember      
   {
   }


   public interface IAggregateMember<T> : IAggregateMember
      where T : IAggregateRoot
   {      
   }

   public interface IAggregateMember
   {
   }
}