using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.DAL.Model
{
    public class DbContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly GridFSBucket _bucket = null;

        public DbContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);
                _bucket = new GridFSBucket(_database);
            }
                
        }

        public GridFSBucket Bucket => _bucket;
        //{
        //    get { return _bucket; }
        //}

        public IMongoCollection<Order> Orders
        {
            get { return _database.GetCollection<Order>("Orders"); }
        }

        public IMongoCollection<Person> Persons
        {
            get { return _database.GetCollection<Person>("Persons"); }
        }

    }
}
