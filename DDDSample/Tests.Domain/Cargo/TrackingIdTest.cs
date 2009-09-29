using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Domain.Tests.Cargo
{
   [TestClass]
   public class TrackingIdTest
   {
      [TestMethod]
      public void EqualityOperator_BothNulls_TrueReturned()
      {
         TrackingId left = null;
         TrackingId right = null;
         Assert.IsTrue(left == right);
      }
      [TestMethod]
      public void EqualityOperator_LeftNull_FalseReturned()
      {
         TrackingId left = null;
         TrackingId right = new TrackingId("A");
         Assert.IsFalse(left == right);
      }
      [TestMethod]
      public void EqualityOperator_RightNull_FalseReturned()
      {
         TrackingId left = new TrackingId("A");
         TrackingId right = null;
         Assert.IsFalse(left == right);
      }
      [TestMethod]
      public void EqualityOperator_NotEqual_FalseReturned()
      {
         TrackingId left = new TrackingId("B");
         TrackingId right = new TrackingId("A");
         Assert.IsFalse(left == right);
      }
      [TestMethod]
      public void EqualityOperator_Equal_TrueReturned()
      {
         TrackingId left = new TrackingId("A");
         TrackingId right = new TrackingId("A");
         Assert.IsTrue(left == right);
      }
   }
}
