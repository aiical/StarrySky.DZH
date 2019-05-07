using MongoDB.Bson;
using MongoDB.Driver;
using StarrySky.DZH.ORMTool.MongoDB.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.MongoDB
{
    public class MongoDBRepository<T> where T : BaseMongoEntity
    {
        /// <summary>
        /// MongoDB的DBName
        /// </summary>
        private string _dbName = null;
        /// <summary>
        /// MongoDB的表名
        /// </summary>
        private string _collectionName = null;
        /// <summary>
        /// MongoDB的连接字串
        /// </summary>
        private string _connectString = null;


        private static MongoClient _client;
        private MongoClient Client
        {
            get
            {
                if (MongoDBRepository<T>._client == null)
                {
                    MongoDBRepository<T>._client = new MongoClient(this._connectString);
                }
                return MongoDBRepository<T>._client;
            }
        }

        public MongoDBRepository(string collectionName = "", string connStr = "", string dBName = "")
        {
            if (string.IsNullOrWhiteSpace(dBName))
            {
                this._dbName = "local";//本地默认数据库
            }
            else
            {
                this._dbName = dBName;
            }
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                this._collectionName = typeof(T).Name;
            }
            else
            {
                this._collectionName = collectionName;
            }
            if (string.IsNullOrWhiteSpace(connStr))
            {
                this._connectString = "mongodb://127.0.0.1:27017"; //本地默认数据库地址
            }
            else
            {
                this._connectString = connStr;
            }
        }


        public IMongoDatabase GetDb()
        {
            var db = this.Client.GetDatabase(this._dbName);
            return db;
        }

        public IMongoCollection<T> GetCollection()
        {

            var db = this.Client.GetDatabase(this._dbName);
            return db.GetCollection<T>(this._collectionName);
        }

        public System.Linq.IQueryable<T> GetWhere(System.Linq.Expressions.Expression<System.Func<T, bool>> fun)
        {
            return this.GetCollection().AsQueryable<T>().Where(fun);
        }
        /// <summary>
        /// 插入数据，mongoDB. 自动生成主键objectId
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            this.GetCollection().InsertOne(entity);
        }

        public Task AddAsync(T entity)
        {
            return this.GetCollection().InsertOneAsync(entity);
        }

        public void AddBatch(List<T> list)
        {
            this.GetCollection().InsertManyAsync(list);
        }

        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public UpdateResult UpdateField(string objectId, string field, string value)
        {
            var filter = Builders<T>.Filter.Eq("Id", objectId);
            var updated = Builders<T>.Update.Set(field, value);
            return this.GetCollection().UpdateOne(filter, updated);
        }
        /// <summary>
        /// 替换单条表记录
        /// </summary>
        /// <param name="entity"></param>
        public void ReplaceEntity(T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            this.GetCollection().ReplaceOne(filter, entity);
        }
        /// <summary>
        /// 更新实体已修改字段，未修改字段不变
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(T entity)
        {
            try
            {
                var old = this.GetCollection().Find(e => e.Id.Equals(entity.Id)).ToList().FirstOrDefault();
                if (old == null)
                    return;
                foreach (var prop in entity.GetType().GetProperties())
                {
                    var newValue = prop.GetValue(entity);
                    var oldValue = old.GetType().GetProperty(prop.Name).GetValue(old);
                    if (newValue != null)
                    {
                        if (oldValue == null)
                            oldValue = "";
                        if (!newValue.ToString().Equals(oldValue.ToString()))
                        {
                            old.GetType().GetProperty(prop.Name).SetValue(old, newValue);
                        }
                    }
                }
                var filter = Builders<T>.Filter.Eq("Id", entity.Id);
                this.GetCollection().ReplaceOne(filter, old);

            }
            catch (Exception ex)
            {
            }

        }
        public bool Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            return this.GetCollection().DeleteOne(filter).DeletedCount == (long)1;
        }
        public bool Delete(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return this.GetCollection().DeleteOne(filter).DeletedCount == (long)1;
        }

        public T QueryById(ObjectId id)
        {
            return this.GetCollection().Find(e => e.Id == id).ToList().FirstOrDefault();
        }

        public List<T> Query(Expression<Func<T, bool>> express)
        {
            return this.GetCollection().Find(express).ToList();
        }
    }
}
