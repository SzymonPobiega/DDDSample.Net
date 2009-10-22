//using System;
//using System.Linq;
//using System.Collections.Generic;
//using DDDSample.Domain.Voyage;

//namespace DDDSample.Domain.Persistence.InMemory
//{
//   public class VoyageRepository : IVoyageRepository
//   {
//      private static readonly List<Voyage.Voyage> _storage = new List<Voyage.Voyage>();

//      public Voyage.Voyage Find(VoyageNumber voyageNumber)
//      {
//         return _storage.FirstOrDefault(x => x.Number == voyageNumber);
//      }
//   }
//}