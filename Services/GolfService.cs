using GolfApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace GolfApi.Services
{
	public class GolfService
	{
		private readonly IMongoCollection<User> _golf;

		public GolfService(IGolfDBSettings settings)
		{
			//get connection to db
			var client = new MongoClient(settings.ConnectionString);
			var db = client.GetDatabase(settings.DatabaseName);
			_golf = db.GetCollection<User>(settings.GolfCollectionName);
		}

		public List<User> Get() => _golf.Find(user => true).ToList();

		public User GetUser(string userName)
		{
			return _golf.Find(u => u.Name.ToLower() == userName.ToLower()).FirstOrDefault();
		}

		public List<User> GetGames(string gameId)
		{
			return _golf.Find(user => user.Games.Any(g => g.GameId == gameId))
				.Project(Builders<User>.Projection
					.ElemMatch(x => x.Games, gm => gm.GameId == gameId)
					.Include(x => x.Name))
					.ToEnumerable()
					.Select(b => BsonSerializer.Deserialize<User>(b)).ToList();
		}

		//Get a user and the game requested
		public User Get(string userName, string gameId)
		{
			return _golf.Find(user => user.Games.Any(g => g.GameId == gameId))
					.Project(Builders<User>.Projection
							.ElemMatch(x => x.Games, z => z.GameId == gameId)
							.Include(x => x.Name))
					.ToEnumerable()
					.Select(b => BsonSerializer.Deserialize<User>(b))
					.Where(u => u.Name.ToLower() == userName.ToLower())
					.FirstOrDefault();
		}

		//Deletes one User/User Object 
		public void Delete(string id)
		{
			_golf.DeleteOne(g => g.Id == id);
		}

		//Remove a game from all the users who have a reference to it
		public void DeleteGame(string gameId)
		{
			//This is really an update call because we remove one sub document but keep the document.
			var filter = Builders<User>.Filter.ElemMatch(x => x.Games, gm => gm.GameId == gameId);
			var pullFilter = Builders<User>.Update.PullFilter(g => g.Games, gm => gm.GameId == gameId);
			_golf.UpdateMany(filter, pullFilter);
		}

		public User Create(User user)
		{
			_golf.InsertOne(user);
			return user;
		}
		//This almost works
		public void CreateGame(string userId, Game newgame)
		{
			//autoMapping? 
			// var game = new Game()
			// {
			// 	GameId = newgame.Id,

			// }
			var _id = MongoDB.Bson.ObjectId.Parse(userId);
			var filter = Builders<User>.Filter.Eq(x => x.Id, _id.ToString());
			var update = Builders<User>.Update.AddToSet(x => x.Games, newgame);
			_golf.UpdateOne(filter, update);
		}
	}


}