namespace CourseApi.Models
{
    public class OrderstoreDatabaseSettings : IOrderstoreDatabaseSettings
    {
        public string OrdersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }   
    }

    public interface IOrderstoreDatabaseSettings
    {
         string OrdersCollectionName { get; set; }
         string ConnectionString { get; set; }
         string DatabaseName { get; set; }   
    }
}