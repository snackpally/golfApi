using GolfApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace GolfApi.Services
{
	public class GolfService
	{
		private readonly IMongoCollection<Golf> _golf;

		public GolfService(IGolfDBSettings settings)
		{
			//get connection to db
			var client = new MongoClient(settings.ConnectionString);
			var db = client.GetDatabase(settings.DatabaseName);
			_golf = db.GetCollection<Golf>(settings.GolfCollectionName);
		}

		public List<Golf> Get() => _golf.Find(golf => true).ToList();

		public Golf GetUser(string userName)
		{
			return _golf.Find(u => u.Name.ToLower() == userName.ToLower()).FirstOrDefault();
		}

		public List<Golf> GetGames(string gameId)
		{
			return _golf.Find(golf => golf.Games.Any(g => g.GameId == gameId))
				.Project(Builders<Golf>.Projection
					.ElemMatch(x => x.Games, gm => gm.GameId == gameId)
					.Include(x => x.Name))
					.ToEnumerable()
					.Select(b => BsonSerializer.Deserialize<Golf>(b)).ToList();
		}

		//Get a user and the game requested
		public Golf Get(string userName, string gameId)
		{
			return _golf.Find(golf => golf.Games.Any(g => g.GameId == gameId))
					.Project(Builders<Golf>.Projection
							.ElemMatch(x => x.Games, z => z.GameId == gameId)
							.Include(x => x.Name))
					.ToEnumerable()
					.Select(b => BsonSerializer.Deserialize<Golf>(b))
					.Where(u => u.Name.ToLower() == userName.ToLower())
					.FirstOrDefault();
		}

		//Deletes one Golf/User Object 
		public void Delete(string id)
		{
			_golf.DeleteOne(g => g.Id == id);
		}

		//Remove a game from all the users who have a reference to it
		public void DeleteGame(string gameId)
		{
			//This is really an update call because we remove one sub document but keep the document.
			var filter = Builders<Golf>.Filter.ElemMatch(x => x.Games, gm => gm.GameId == gameId);
			var pullFilter = Builders<Golf>.Update.PullFilter(g => g.Games, gm => gm.GameId == gameId);
			_golf.UpdateMany(filter, pullFilter);
		}

		public Golf Create(Golf golf)
		{
			_golf.InsertOne(golf);
			return golf;
		}
	}


}