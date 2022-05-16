using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTEDoc.DataRepository.Data;
using NTEDoc.Models;
using NTEDoc.Models.ViewModels;

namespace NTEDoc.Controllers
{
    [Route("[controller]")]
    public class ContractController : Controller
    {
        private readonly EntityDbContext _context;

        public ContractController(EntityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }


        [HttpPost("ContractsData")]
        public IActionResult GetUsersData([FromBody] ContractDataTable dataTableParameters) 
        {
            var recordsTotal = 0;
            var draw = dataTableParameters.Draw;
            var searchValue = dataTableParameters.Search.Value.ToLower();
        
            var skipValue = dataTableParameters.Start;
            var takeValue = dataTableParameters.Length;

            var contracts = _context.Contracts.Include("Documents").ToList();
            recordsTotal = contracts.Count();

            if (!string.IsNullOrEmpty(searchValue)) 
            {
                contracts = contracts.Where(x => x.Name.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase) ||
                        x.ArchiveNumber.Contains(searchValue, StringComparison.CurrentCultureIgnoreCase))
                        .ToList();
            }

            var contractsViews = contracts.Select(x => new ContractViewModel {
                CompanyContractId = x.CompanyContractId,
                ContractId = x.ContractId,
                ArchiveNumber = x.ArchiveNumber,
                Name = x.Name,
                CompanyId = x.CompanyId,
                ContractDate = x.ContractDate.Value.ToString("dd-MM-yyyy"),
                Description = x.Description,
                OwnerId = x.OwnerId,
                Active = x.Active,
                EndingDeadline = x.EndingDeadline.Value.ToString("dd-MM-yyyy"),
                ContractTypeId = x.ContractTypeId,
                AnnexId = x.AnnexId,
                ExecutorId = x.ExecutorId,
                TotalSum = x.Documents.Aggregate(0, (prev, element) => prev + ((element.DocumentTypeId != 3 && element.DocumentTypeId != 8) ? Convert.ToInt32(element.Amount) : 0))
            });

            contractsViews = contractsViews.Skip(skipValue).Take(takeValue).ToList();

            var json = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = contractsViews };
            return Json(json);
        }
    }
}