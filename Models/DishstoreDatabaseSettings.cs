namespace CourseApi.Models
{
    public class DishstoreDatabaseSettings : IDishstoreDatabaseSettings
    {
        public string DishesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }   
    }

    public interface IDishstoreDatabaseSettings
    {
        string DishesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }   
    }
}