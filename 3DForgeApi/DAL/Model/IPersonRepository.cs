using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.DAL.Model
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllPersons();
        Task<Person> GetPerson(string id);
        Task AddPerson(Person person);
        Task<DeleteResult> RemovePerson(string id);
        Task<UpdateResult> UpdatePerson(string id, Person person);
    }
}
