using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GolfApi.Models
{
	public class Hole
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement("modified")]
		public DateTime Modified { get; set; }
		[BsonElement("par")]
		public int Par { get; set; }
		[BsonElement("strokes")]
		public int Strokes { get; set; }
		[BsonElement("putts")]
		public int Putts { get; set; }
		[BsonElement("fairwayHit")]
		public bool FairwayHit { get; set; }
		[BsonElement("greensInRegulation")]
		public bool GreensInRegulation { get; set; }
		[BsonElement("picture")]
		public string Picture { get; set; }

	}
}