using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.Services
{
    public interface IPasswordService
    {
        string GetPasswordHash(string input);
    }
}
