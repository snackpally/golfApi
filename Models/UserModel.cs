using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GolfApi.Models
{
	[BsonIgnoreExtraElements]
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement("name")]
		public string Name { get; set; }
		[BsonElement("games")]
		public List<Game> Games { get; set; }
	}

}