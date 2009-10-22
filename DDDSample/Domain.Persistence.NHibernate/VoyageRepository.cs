//using System;
//using DDDSample.Domain.Voyage;
//using NHibernate;

//namespace DDDSample.Domain.Persistence.NHibernate
//{
//   /// <summary>
//   /// Voyage repository implementation based on NHibernate.
//   /// </summary>
//   public class VoyageRepository : AbstractRepository, IVoyageRepository
//   {
//      public VoyageRepository(ISessionFactory sessionFactory)
//         : base(sessionFactory)
//      {
//      }      

//      public Voyage.Voyage Find(VoyageNumber voyageNumber)
//      {
//         const string query = @"from DDDSample.Domain.Voyage.Voyage v where v.Number = :number";
//         return Session.CreateQuery(query).SetString("number", voyageNumber.NumberString)
//            .UniqueResult<Voyage.Voyage>();
//      }
//   }
//}