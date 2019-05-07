using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.ORMTool.MongoDB.Entity
{
    public class BaseMongoEntity
    {
        public ObjectId Id { get; set; }

    }
}
