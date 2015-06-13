using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace CqrsSeed.BC.Inventory.ReadModel.Mongo
{
    public class MongoRepository
    {
        private readonly MongoDatabase _db;

        public MongoRepository(MongoDatabase db)
        {
            _db = db;
        }

        protected MongoCollection<TModel> Collection<TModel>()
        {
            return _db.GetCollection<TModel>(typeof(TModel).Name);
        }

        #region Public methods

        public IEnumerable<TModel> FindAll<TModel>() where TModel : class, IMongoEntity
        {
            return FindAll<TModel>(null);
        }

        public IEnumerable<TModel> FindAll<TModel>(string[] keys) where TModel : class, IMongoEntity
        {
            var cursor = Collection<TModel>().FindAllAs<TModel>();

            if (keys != null)
                cursor.SetSortOrder(keys);

            var documents = cursor.ToList();

            return documents;
        }

        public IEnumerable<TModel> Find<TModel>(IMongoQuery query) where TModel : class, IMongoEntity
        {
            return this.Collection<TModel>().Find(query).ToList();
        }

        public TModel FindOne<TModel>(Guid id) where TModel : class, IMongoEntity
        {
            var document = Collection<TModel>().FindOneById(id);

            return document;
        }

        public void Insert<TModel>(TModel model) where TModel : class, IMongoEntity
        {
            Collection<TModel>().Insert(model, WriteConcern.Acknowledged);
        }

        public void Update<TModel>(Guid id, UpdateBuilder<TModel> builder) where TModel : class, IMongoEntity
        {
            Collection<TModel>().Update(Query<TModel>.EQ(x => x.Id, id), builder);
        }

        public void Remove<TModel>(Guid id) where TModel : class, IMongoEntity
        {
            Collection<TModel>().Remove(Query<TModel>.EQ(x => x.Id, id));
        }

        #endregion Public methods
    }
}