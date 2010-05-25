using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class PropertiesOnlyContractResolver : DefaultContractResolver
   {
      protected override List<MemberInfo> GetSerializableMembers(Type objectType)
      {
         var result = base.GetSerializableMembers(objectType);
         return result.Where(x => x.MemberType == MemberTypes.Property).ToList();
      }
   }
}