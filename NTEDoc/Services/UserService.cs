using Microsoft.EntityFrameworkCore;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace NTEDoc.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var queryString = $"SELECT * FROM Users WHERE Username = '{username}' AND Password = dbo.FN_Korisnik('{password}')";
            var user = await _context.Users.FromSqlRaw(queryString).SingleOrDefaultAsync();

            return user;
        }
    }
}
