using System;
using Newtonsoft.Json;

namespace DDDSample.Domain.Persistence.NHibernate
{
   public class Event
   {
      public string Id { get; set; }      
      public int Version { get; set; }
      public string EntityId { get; set; }
      [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
      public object Data { get; set; }
      public bool IsSnapshot { get; set; }
   }
}