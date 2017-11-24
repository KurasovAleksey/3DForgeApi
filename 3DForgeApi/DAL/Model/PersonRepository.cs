using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace _3DForgeApi.DAL.Model
{
    public class PersonRepository : IPersonRepository
    {
        private DbContext _context = null;

        public PersonRepository(DbContext context)
        {
            _context = context;
        }

        public async Task AddPerson(Person person)
        {
            await _context.Persons.InsertOneAsync(person);
        }

        public async Task<IEnumerable<Person>> GetAllPersons()
        {
            return await _context.Persons.Find(_ => true).ToListAsync();
        }

        public async Task<Person> GetPerson(string id)
        {
            var filter = Builders<Person>.Filter.Eq("Id", id);
            return await _context.Persons
                 .Find(filter)
                 .FirstOrDefaultAsync();

        }

        public async Task<DeleteResult> RemovePerson(string id)
        {
            var filter = Builders<Person>.Filter.Eq("Id", id);
            return await _context.Persons.DeleteOneAsync(filter);
        }

        public async Task<UpdateResult> UpdatePerson(string id, Person person)
        {
            var filter = Builders<Person>.Filter.Eq("Id", id);
            var update = Builders<Person>.Update
                .Set(p => p.Login, person.Login)
                .Set(p => p.PasswordHash, person.PasswordHash)
                .Set(p => p.Role, person.Role);
            return await _context.Persons.UpdateOneAsync(filter, update);
        }
    }
}
