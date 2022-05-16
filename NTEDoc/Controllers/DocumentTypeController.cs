using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.UnitOfWork;

namespace NTEDocSystemV2.Controllers
{
    public class DocumentTypeController : Controller
    {
        private IUnitOfWork<EntityDbContext> _unitOfWork;

        public DocumentTypeController(IUnitOfWork<EntityDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _unitOfWork.DocTypeRepository.GetAll().ToList();

            return View(model);
        }
    }
}