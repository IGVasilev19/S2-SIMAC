using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class PasswordHasher
    {
        public static string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public static bool Verify(string password, string accountPassword) => BCrypt.Net.BCrypt.Verify(password, accountPassword);
    }
}
