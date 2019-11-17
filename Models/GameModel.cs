using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GolfApi.Models
{
	public class Game
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement("players")]
		public List<string> Players { get; set; }
		[BsonElement("gameId")]
		public string GameId { get; set; }
		[BsonElement("courseName")]
		public string CourseName { get; set; }
		[BsonElement("active")]
		public bool Active { get; set; }
		[BsonElement("holes")]
		public List<Hole> Holes { get; set; }
		[BsonElement("date")]
		public DateTime Date { get; set; }
		[BsonElement("formattedDate")]
		public string FormattedDate { get; set; }

	}
}