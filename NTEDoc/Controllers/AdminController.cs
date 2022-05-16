using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.Models;
using NTEDoc.Models;

namespace NTEDoc.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles="99")]
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UsersData")]
        public IActionResult GetUsersData([FromBody] UsersDataTable dataTableParameters) 
        {
            var recordsTotal = 0;
            var draw = dataTableParameters.Draw;
            var searchValue = dataTableParameters.Search.Value.ToLower();
        
            var users = _context.Users.ToList();
            recordsTotal = users.Count();

            if (!string.IsNullOrEmpty(searchValue)) 
            {
                users = users.Where(
                    x => x.Username.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase) || 
                    x.FullName.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            var json = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = users };
            return Json(json);
        }
    }
}