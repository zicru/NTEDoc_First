﻿using NTEDoc.DataRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTEDoc.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);

    }
}
