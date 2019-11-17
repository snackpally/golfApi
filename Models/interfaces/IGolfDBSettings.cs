
namespace GolfApi.Models
{
	public interface IGolfDBSettings
	{
		string GolfCollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}

}