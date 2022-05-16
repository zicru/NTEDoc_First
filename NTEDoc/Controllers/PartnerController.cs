using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NTEDoc.DataRepository.Data;
using NTEDoc.Models;

namespace NTEDoc.Controllers
{
    [Route("[controller]")]
    public class PartnerController : Controller
    {
        private readonly EntityDbContext _context;

        public PartnerController(EntityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet("{partnerId}")]
        public IActionResult Details([FromRoute] int partnerId)
        {
            var partnerToDisplay = _context.Partner.Find(partnerId);

            return View(partnerToDisplay);
        }


        [HttpPost("PartnersData")]
        public IActionResult GetUsersData([FromBody] PartnerDataTable dataTableParameters) 
        {
            var recordsTotal = 0;
            var draw = dataTableParameters.Draw;
            var searchValue = dataTableParameters.Search.Value.ToLower();
        
            var skipValue = dataTableParameters.Start;
            var takeValue = dataTableParameters.Length;

            var partners = _context.Partner.ToList();
            recordsTotal = partners.Count();

            if (!string.IsNullOrEmpty(searchValue)) 
            {
                partners = partners.Where(x => x.naziv.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase) ||
                        x.Konto.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();
            }

            partners = partners.Skip(skipValue).Take(takeValue).ToList();

            var json = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = partners };
            return Json(json);
        }
    }
}