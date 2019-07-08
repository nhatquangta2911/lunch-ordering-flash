namespace CourseApi.Models
{
   public class AlbumstoreDatabaseSettings : IAlbumstoreDatabaseSettings
   {
      public string AlbumsCollectionName { get; set; }
      public string ConnectionString { get; set; }
      public string DatabaseName { get; set; }
   }

   public interface IAlbumstoreDatabaseSettings
   {
      string AlbumsCollectionName { get; set; }
      string ConnectionString { get; set; }
      string DatabaseName { get; set; }
   }
}