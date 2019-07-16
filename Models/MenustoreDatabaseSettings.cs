using System.Collections.Generic;
using CourseApi.Entities;

namespace CourseApi.Models
{
   public class MenustoreDatabaseSettings : IMenustoreDatabaseSettings
   {
      public string MenusCollectionName { get; set; }
      public string ConnectionString { get; set; }
      public string DatabaseName { get; set; }
   }

   public interface IMenustoreDatabaseSettings
   {
      string MenusCollectionName { get; set; }
      string ConnectionString { get; set; }
      string DatabaseName { get; set; }
   }
}