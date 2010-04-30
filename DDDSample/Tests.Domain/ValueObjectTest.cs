using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain;
using NUnit.Framework;

namespace Domain.Tests
{
   [TestFixture]
   public class ValueObjectTest
   {      
      [Test]
      public void EqualityOperator_BothNulls_TrueReturned()
      {
         FakeValueObject left = null;
         FakeValueObject right = null;
         Assert.IsTrue(left == right);
      }
      [Test]
      public void EqualityOperator_LeftNull_FalseReturned()
      {
         FakeValueObject left = null;
         FakeValueObject right = new FakeValueObject("A");
         Assert.IsFalse(left == right);
      }
      [Test]
      public void EqualityOperator_RightNull_FalseReturned()
      {
         FakeValueObject left = new FakeValueObject("A");
         FakeValueObject right = null;
         Assert.IsFalse(left == right);
      }
      [Test]
      public void EqualityOperator_NotEqual_FalseReturned()
      {
         FakeValueObject left = new FakeValueObject("B");
         FakeValueObject right = new FakeValueObject("A");
         Assert.IsFalse(left == right);
      }
      [Test]
      public void EqualityOperator_Equal_TrueReturned()
      {
         FakeValueObject left = new FakeValueObject("A");
         FakeValueObject right = new FakeValueObject("A");
         Assert.IsTrue(left == right);
      }

      [Test]
      public void InequalityOperator_BothNulls_FalseReturned()
      {
          FakeValueObject left = null;
          FakeValueObject right = null;
          Assert.IsFalse(left != right);
      }
      [Test]
      public void InequalityOperator_LeftNull_TrueReturned()
      {
          FakeValueObject left = null;
          FakeValueObject right = new FakeValueObject("A");
          Assert.IsTrue(left != right);
      }
      [Test]
      public void InequalityOperator_RightNull_TrueReturned()
      {
          FakeValueObject left = new FakeValueObject("A");
          FakeValueObject right = null;
          Assert.IsTrue(left != right);
      }
      [Test]
      public void InequalityOperator_NotEqual_TrueReturned()
      {
          FakeValueObject left = new FakeValueObject("B");
          FakeValueObject right = new FakeValueObject("A");
          Assert.IsTrue(left != right);
      }
      [Test]
      public void InequalityOperator_Equal_FalseReturned()
      {
          FakeValueObject left = new FakeValueObject("A");
          FakeValueObject right = new FakeValueObject("A");
          Assert.IsFalse(left != right);
      }

      [Test]
      public void GetHashCode_SingleValue_ThisValueHashCodeReturned()
      {
         string singleValue = "abcd";
         FakeValueObject obj = new FakeValueObject(singleValue);

         Assert.AreEqual(singleValue.GetHashCode(), obj.GetHashCode());
      }

      [Test]
      public void GetHashCode_NullValue_ZeroReturned()
      {         
         FakeValueObject obj = new FakeValueObject(new object[] {null});

         Assert.AreEqual(0, obj.GetHashCode());
      }

      [Test]
      public void GetHashCode_TwoValues_XorOfHashCodesReturned()
      {
         string firstValue = "abcd";
         int secodValue = 15;
         FakeValueObject obj = new FakeValueObject(firstValue, secodValue);

         Assert.AreEqual(firstValue.GetHashCode() ^ secodValue.GetHashCode(), obj.GetHashCode());
      }

      private class FakeValueObject : ValueObject
      {
         private readonly List<object> _fakeValues;

         public FakeValueObject(params object[] fakeValues)
         {
            _fakeValues = new List<object>(fakeValues);
         }

         protected override IEnumerable<object> GetAtomicValues()
         {
            return _fakeValues;
         }

         public static bool operator ==(FakeValueObject left, FakeValueObject right)
         {
            return EqualOperator(left, right);
         }

         public static bool operator !=(FakeValueObject left, FakeValueObject right)
         {
            return NotEqualOperator(left, right);
         }
      }
   }
}
