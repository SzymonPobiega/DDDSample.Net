//using System;

//namespace DDDSample.DomainModel.Potential.Voyage
//{
//   public class Test
//   {
//      public void TestSchedule()
//      {
//         var schedule = ScheduleBuilder
//            .On(DateTime.Now)
//            .DepartFrom(null)
//            .ThenGoTo(null, 4)
//            .WaitFor(2)
//            .ThenGoTo(null, 4)
//            .Build();
//      }
//   }

//   public class ScheduleBuilder : IPause, IDepartFrom
//   {
//      private ScheduleBuilder(Location.Location origin)
//      {
         
//      }

//      public static IDepartFrom On(DateTime departureDate)
//      {
//         return new ScheduleBuilder(null);
//      }      

//      public IPause ThenGoTo(Location.Location destination, int voyageDurationInDays)
//      {
//         return this;
//      }      

//      public IThenGoTo WaitFor(int pauseDurationInDays)
//      {
//         return this;
//      }

//      public Schedule Build()
//      {
//         throw new NotImplementedException();
//      }

//      public IPause DepartFrom(Location.Location destination)
//      {
//         return this;
//      }
//   }

//   public interface IDepartFrom
//   {
//      IPause DepartFrom(Location.Location destination);
//   }

//   public interface IThenGoTo
//   {
//      IPause ThenGoTo(Location.Location destination, int voyageDurationinDays);
//   }   

//   public interface IPause : IThenGoTo
//   {
//      IThenGoTo WaitFor(int pauseDurationInDays);
//      Schedule Build();
//   }
//}