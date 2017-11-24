using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _3DForgeApi.DAL.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3DForgeApi.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        // GET: api/values
        private readonly IPersonRepository _personRepository = null;

        public AdminController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
            
        }

       

        
    }
}
