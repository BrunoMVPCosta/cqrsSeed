using System;
using MongoDB.Bson.Serialization.Attributes;

namespace CqrsSeed.BC.Inventory.ReadModel.Mongo
{
    public interface IMongoEntity
    {
        Guid Id { get; set; }
    }
    public class MongoEntity: IMongoEntity
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
