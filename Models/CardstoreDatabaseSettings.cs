namespace CourseApi.Models {
   public class CardstoreDatabaseSettings : ICardstoreDatabaseSettings
   {
        public string CardsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }  
   }

   public interface ICardstoreDatabaseSettings 
    {
        string CardsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}