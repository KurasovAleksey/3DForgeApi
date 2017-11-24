using _3DForgeApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _3DForgeApi.Services
{
    public class PasswordService : IPasswordService
    {
        public string GetPasswordHash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (var item in hash)
                sb.Append(item.ToString("x2"));

            return sb.ToString();
        }
    }
}
