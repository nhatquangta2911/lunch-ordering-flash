namespace CourseApi.Models
{
    public class DailyChoicestoreDatabaseSettings : IDailyChoicestoreDatabaseSettings
    {
        public string DailyChoicesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }   
    }

    public interface IDailyChoicestoreDatabaseSettings {
         string DailyChoicesCollectionName { get; set; }
         string ConnectionString { get; set; }
         string DatabaseName { get; set; }   
    }
}