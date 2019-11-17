namespace GolfApi.Models
{
	public class DbSettings : IGolfDBSettings
	{
		public string GolfCollectionName { get; set; }
		public string ConnectionString { get; set; }
		public string DatabaseName { get; set; }
	}

}