namespace Enter_The_Matrix.Models
{
    public class ETMDatabaseSettings : IETMDatabaseSettings
    {
        public string AssessmentsCollectionName { get; set; }
        public string ScenariosCollectionName { get; set; }
        public string StepsCollectionName { get; set; }
        public string SteplatesCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string TreesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IETMDatabaseSettings
    {
        string AssessmentsCollectionName { get; set; }
        string ScenariosCollectionName { get; set; }
        string StepsCollectionName { get; set; }
        string SteplatesCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string TreesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
