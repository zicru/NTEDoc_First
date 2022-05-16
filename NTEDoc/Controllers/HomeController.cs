using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NTEDoc.DataRepository;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.UnitOfWork;
using NTEDoc.Models;
using NTEDoc.Models.ViewModels;

namespace NTEDoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IUnitOfWork<EntityDbContext> _unitOfWork;

        private RoleManager<IdentityRole> RoleManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork<EntityDbContext> unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            RoleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            //var role = "Kontrolor";
            //var roleCheck = await RoleManager.RoleExistsAsync(role);
            //IdentityResult roleResult;

            //if (!roleCheck)
            //{
            //    //create the roles and seed them to the database
            //    roleResult = await RoleManager.CreateAsync(new IdentityRole(role));

            //}
            //return View();
            return RedirectToAction("Index", "Document");
        }

        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data
                var customerData =  _unitOfWork.DocumentRepository.GetAll(new[] { "Sector", "DocumentType", "Partner" }) as IQueryable<Document>;

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Name.Contains(searchValue)
                    || m.DocumentNumber.Contains(searchValue)
                    || m.Partner.naziv.Contains(searchValue));
                }

                //total number of rows count 
                recordsTotal = customerData.Count();
                //Paging 
                var myData = customerData.Skip(skip).Take(pageSize).Select(s => new DocumentLight
                {
                    Id = s.Id,
                    Name = s.Name,
                    DocumentType = s.DocumentType.Naziv,
                    DocumentFileName = s.DocumentFiles.Where(f => f.IsPrimaryFile == 1).SingleOrDefault().FileName,
                    DocumentNumber = s.DocumentNumber,
                    Sector = s.Sector.Naziv,
                    Partner = s.Partner.naziv
                }).ToArray();

                
                //Returning Json Data
                var json = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = myData };
                //draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal,
                return Json(json);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        
     

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class StringHelper
    {
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }
}
