using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NTEDoc.Data;
using NTEDoc.DataRepository;
using NTEDoc.DataRepository.Data;
using NTEDoc.DataRepository.GenericRepository;
using NTEDoc.DataRepository.Models;
using NTEDoc.DataRepository.NonGenericRepository;
using NTEDoc.DataRepository.UnitOfWork;
using NTEDoc.Models;
using NTEDoc.Models.ViewModels;
using NTEDoc.Services;
using ServiceStack.Auth;
using ServiceStack;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;
using jsreport.AspNetCore;
using jsreport.Types;
using NTEDocOriginal.Models;

namespace NTEDocSystemV2.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private IUnitOfWork<EntityDbContext> _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly EntityDbContext _entityContext;
        private readonly UZZPContext _uzzpContext;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        private readonly DataContext _dataContext;

        public DocumentController(IUnitOfWork<EntityDbContext> unitOfWork, ApplicationDbContext context, UZZPContext uzzpContext,
            EntityDbContext entityContext, IFileService fileService, IConfiguration configuration, DataContext dataContext)

        {
            _unitOfWork = unitOfWork;
            _context = context;
            _uzzpContext = uzzpContext;
            _entityContext = entityContext;
            _fileService = fileService;
            _configuration = configuration;

            _dataContext = dataContext;
        }

        public IActionResult Index()
        {

            List<Partner> dropListPartner = new List<Partner>();
            dropListPartner = _unitOfWork.PartnerRepository.GetAll().ToList();

            ViewBag.Partners = dropListPartner;
            ViewBag.PartnerList = new SelectList(dropListPartner, "Id", "naziv");

            ViewBag.Recorders = _entityContext.Users.Where(u => u.RoleId == UserRoles.Recorder.ToString());

            //List<PartneriUgovori> dropList = new List<PartneriUgovori>();
            //var firmaIdList = _uzzpContext.V_firme_ugovori.Select(s => s.IDFirme).ToList();

            ViewBag.CanCreateDocuments = UserHasPermission(UserPermissions.CanAddOrUpdateDocuments);

            List<Sector> dropListSector = new List<Sector>();
            dropListSector = _unitOfWork.SectorRepository.GetAll().ToList();

            ViewBag.Sectors = dropListSector;
            ViewBag.SectorList = new SelectList(dropListSector, "Id", "Naziv");

            List<DocumentType> dropListType = new List<DocumentType>();
            dropListType = _unitOfWork.DocTypeRepository.GetAll().ToList();
            ViewBag.DocumentTypes = dropListType;
            ViewBag.TypeList = new SelectList(dropListType, "Id", "Naziv");

            var list = new SelectList(Enumerable.Range(DateTime.Today.Year - 9, 10).Select(x =>
              new SelectListItem()
              {
                  Text = x.ToString(),
                  Value = x.ToString()
              }), "Value", "Text");

            ViewBag.Years = list.Reverse();

            //ako User-u nije dodeljen sektor, onda mu prikazujemo na View-u samo dugme za prosledjivanje
            //var user = _context.ApplicationUser.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault();

            try
            {
                var user = GetUserFromToken();

                var sektor = _unitOfWork.SectorRepository.GetById(user.OrganisationUnitId.ToInt());
                ViewBag.Sektor = sektor != null ? sektor.Naziv : null;
                ViewBag.UserId = user.UserId;
                ViewBag.UserRole = user.RoleId.ToInt();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /*if (!HttpContext.User.IsInRole("Likvidator") && !HttpContext.User.IsInRole("Kontrolor"))
            {
                ViewBag.Role = null;
            }
            else
            {
                ViewBag.Role = String.Empty;
            }*/

            var executorsList = _dataContext.Users
                .Where(u => u.RoleId == UserRoles.Executor.ToString())
                .Select(e => new User
                {
                    UserId = e.UserId,
                    FullName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.FullName.ToLower())
                })
                .ToList();

            ViewBag.Executors = executorsList;
            ViewBag.ExecutorList = new SelectList(executorsList, "UserId", "FullName");

            var controllersList = _dataContext.Users
                .Where(u => u.RoleId == UserRoles.JNController.ToString())
                .Select(e => new User
                {
                    UserId = e.UserId,
                    FullName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.FullName.ToLower())
                })
                .ToList();

            ViewBag.Controllers = controllersList;
            ViewBag.ControllerList = new SelectList(controllersList, "UserId", "FullName");
            
            var contracts = _entityContext.Contracts.Select(c => new
            {
                Value = c.CompanyContractId.ToString(),
                Text = $"{c.ArchiveNumber} - {(c.ContractDate.HasValue ? c.ContractDate.Value.ToString("dd/MM/yyyy") : "")}"
            }).ToList();

            ViewBag.CompanyContractsList = new SelectList(contracts, "Value", "Text");

            var deliveryTypes = _entityContext.DeliveryTypes.Select(d => new
            {
                Value = d.DeliveryTypeId,
                Text = d.Name
            }).ToList();

            ViewBag.DeliveryTypes = _entityContext.DeliveryTypes.ToList();
            ViewBag.DeliveryTypeList = new SelectList(deliveryTypes, "Value", "Text");

            List<Status> statusList = new List<Status>();
            statusList = _unitOfWork.StatusRepository.GetAll().ToList();

            ViewBag.Statuses = _unitOfWork.StatusRepository.GetAll().ToList();

            return View(_unitOfWork.DocumentRepository.GetAll(new[] { "Sector", "DocumentType", "Partner", "CompanyContract" }));
        }


        public JsonResult GetBrUgovora(int idPartnera)
        {
            var partner = _unitOfWork.PartnerRepository.GetById(idPartnera);

            List<CompanyContract> dropList = new List<CompanyContract>();

            var contracts = _entityContext;
            dropList = _entityContext.Contracts.Where(u => u.CompanyId == partner.IDFirme && u.Active.HasValue ? u.Active.Value == 1 : false).OrderByDescending(x => x.ContractDate).ToList();

            var list = new List<object>();

            foreach (var item in dropList)
            {
                list.Add(new
                {
                    Text = $"{item.ArchiveNumber} - {(item.ContractDate.HasValue ? item.ContractDate.Value.ToString("dd/MM/yyyy") : "")}",
                    Value = item.CompanyContractId.ToString(),
                    Executor = item.ExecutorId,
                    Controller = item.ControllerId
                });
            }

            return Json(list);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost("/Document/CheckExisting")]
        public IActionResult CheckExisting([FromForm] CheckValuesRequest valuesRequest)
        {
            var document = _entityContext.Document.Where(x =>
                    x.DocumentTypeId == valuesRequest.DocumentType &&
                    x.PartnerId == valuesRequest.Partner &&
                    x.DocumentNumber == valuesRequest.DocumentNumber
                ).FirstOrDefault();

            var response = new
            {
                DocumentExists = document != null
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Document model, IFormFile documentFile)
        {
            if (ModelState.IsValid && UserHasPermission(UserPermissions.CanAddOrUpdateDocuments))
            {
                var user = GetUserFromToken();

                model.CreatedByUserId = user.UserId;
                model.CreatedDate = DateTime.Now.Date;
                model.LastStatusChangeDate = DateTime.Now.Date;
                model.StatusId = 0;

                _unitOfWork.DocumentRepository.Insert(model);
                _unitOfWork.Save();

                if (documentFile != null && documentFile.Length > 0)
                {
                    _entityContext.DocumentFiles.Add(new DocumentFile
                    {
                        DocumentId = model.Id,
                        FileName = await _fileService.SaveFile(documentFile),
                        FileTitle = _entityContext.DocumentType.FirstOrDefault(x => x.Id == model.DocumentTypeId).Naziv.Trim(),
                        IsPrimaryFile = 1,
                        IsActive = 1
                    });
                    _entityContext.SaveChanges();
                }
            }

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

            return RedirectToAction("Index", "Document");
        }


        [HttpPost]
        public async Task<IActionResult> AddFilesToDocument(Document model, IFormFile newDocumentFile, string fileTitle, int isPrimary)
        {
            if (newDocumentFile != null && UserHasPermission(UserPermissions.CanAddOrUpdateDocuments))
            {
                _entityContext.DocumentFiles.Add(new DocumentFile
                {
                    DocumentId = model.Id,
                    FileName = await _fileService.SaveFile(newDocumentFile),
                    FileTitle = fileTitle.Trim(),
                    IsPrimaryFile = Convert.ToByte(isPrimary),
                    IsActive = 1
                });

                _entityContext.SaveChanges();
            }
            else
            {
                // TODO: Add error

            }

            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDocumentFile([FromForm] DocumentFile model, IFormFile newDocumentFile)
        {

            if (newDocumentFile != null)
            {
                _fileService.DeleteFile(model.FileName);
                model.FileName = await _fileService.SaveFile(newDocumentFile);
            }

            _entityContext.DocumentFiles.Update(model);
            _entityContext.SaveChanges();
            return RedirectToAction("Details", new { id = model.DocumentId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocumentFile(int fileId)
        {

            var documentFile = _entityContext.DocumentFiles.Where(x => x.DocumentFileId == fileId).FirstOrDefault();

            documentFile.IsActive = 0;

            if (documentFile != null)
            {
                _entityContext.DocumentFiles.Update(documentFile);
            }

            _entityContext.SaveChanges();
            return RedirectToAction("Details", new { id = documentFile.DocumentId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Document model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.DocumentRepository.Update(model);
                _unitOfWork.Save();
            }

            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            List<Sector> dropListSector = new List<Sector>();
            dropListSector = _unitOfWork.SectorRepository.GetAll().ToList();

            ViewBag.SectorList = new SelectList(dropListSector, "Id", "Naziv");

            List<Partner> dropListPartner = new List<Partner>();
            dropListPartner = _unitOfWork.PartnerRepository.GetAll().ToList();
            ViewBag.PartnerList = new SelectList(dropListPartner, "Id", "naziv");

            List<DocumentType> dropListType = new List<DocumentType>();
            dropListType = _unitOfWork.DocTypeRepository.GetAll().ToList();
            ViewBag.TypeList = new SelectList(dropListType, "Id", "Naziv");

            var deliveryTypes = _entityContext.DeliveryTypes.Select(d => new
            {
                Value = d.DeliveryTypeId,
                Text = d.Name
            }).ToList();
            ViewBag.DeliveryTypeList = new SelectList(deliveryTypes, "Value", "Text");

            Document document = new Document();
            document = _unitOfWork.DocumentRepository.GetById(id, new[] { "Sector", "Partner", "DocumentType", "Status", "Likvidator", "CreatedBy", "CompanyContract", "DocumentFiles", "DeliveryType" }).FirstOrDefault();

            var contracts = _entityContext.Contracts.Where(c => c.CompanyId == document.Partner.IDFirme).Select(c => new
            {
                Value = c.CompanyContractId.ToString(),
                Text = $"{c.ArchiveNumber} - {(c.ContractDate.HasValue ? c.ContractDate.Value.ToString("dd/MM/yyyy") : "")}"
            }).ToList();

            ViewBag.CompanyContractsList = new SelectList(contracts, "Value", "Text");

            //ako User-u nije dodeljen sektor, onda mu prikazujemo na View-u samo dugme za prosledjivanje
            var user = GetUserFromToken();
            var sektor = _unitOfWork.SectorRepository.GetById(user.OrganisationUnitId.ToInt());

            // Setting the property for checking if the document belongs to logged in executor
            ViewBag.AssignedToExecutor = (user.UserId == document.LikvidatorId);

            // Does the document belong to the controller
            ViewBag.BelongsToController = (user.UserId == document.ControllerId);

            // Setting the propery for checking if the document was made by the currently logged in user
            ViewBag.MadeByRecorder = (user.UserId == document.CreatedBy.UserId);

            // Setting user role - has to be done this way to keep it clean.
            ViewBag.UserRole = Int16.Parse(user.RoleId);

            // Setting permissions
            ViewBag.CanAddOrUpdateDocuments = UserHasPermission(UserPermissions.CanAddOrUpdateDocuments);
            ViewBag.CanSendToController = UserHasPermission(UserPermissions.CanSendToController);

            ViewBag.CanEditDocuments = UserHasPermission(UserPermissions.CanEditDocuments);
            ViewBag.CanSendToExecutor = UserHasPermission(UserPermissions.CanSendToExecutor);

            ViewBag.CanConfirmReceiving = UserHasPermission(UserPermissions.CanConfirmReceiving);
            ViewBag.CanSendToSectorInCharge = UserHasPermission(UserPermissions.CanSendToSectorInCharge);
            ViewBag.CanReturnToRecorder = UserHasPermission(UserPermissions.CanReturnToRecorder);
            ViewBag.CanReturnToController = UserHasPermission(UserPermissions.CanReturnToController);
            ViewBag.CanReturnToSupplier = UserHasPermission(UserPermissions.CanReturnToSupplier);

            ViewBag.CanSignAndVerifyDocuments = UserHasPermission(UserPermissions.CanSignAndVerifyDocuments);
            ViewBag.CanApproveDocuments = UserHasPermission(UserPermissions.CanApproveDocuments);

            // Setting the sector
            ViewBag.Sektor = sektor != null ? sektor.Naziv : null;

            // Setting the status
            ViewBag.Status = document.Status.Id;

            // If the status is processed by sector, set the property if it's valid or not
            if (document.Status.Id == DocumentStatus.SectorSignedAndApproved)
            {
                ViewBag.SectorApproved = document.SectorApproved == 1;
            }

            ViewBag.StatusChanges = _entityContext.StatusChanges.Where(l => l.DocumentId == document.Id).Include(e => e.Status).Include(e => e.CreatedBy);

            var executorsList = _dataContext.Users
                .Where(u => u.RoleId == UserRoles.Executor.ToString())
                .Select(e => new User
                {
                    UserId = e.UserId,
                    FullName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.FullName.ToLower())
                })
                .ToList();

            ViewBag.Executors = executorsList;
            ViewBag.ExecutorList = new SelectList(executorsList, "UserId", "FullName");

            var controllersList = _dataContext.Users
                .Where(u => u.RoleId == UserRoles.JNController.ToString())
                .Select(e => new User
                {
                    UserId = e.UserId,
                    FullName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(e.FullName.ToLower())
                })
                .ToList();

            ViewBag.Executors = controllersList;
            ViewBag.ControllerList = new SelectList(controllersList, "UserId", "FullName");

            //List<Log> log = _unitOfWork.LogRepository.GetDocumentLog(id).ToList();
            //ViewBag.Komentar = log.Count() != 0 ? log.Last().Comment : String.Empty;
            //ViewBag.Log = log;
            //return PartialView("_EditPartial", document);


            if (document.CompanyContractId == null)
            {
                document.CompanyContractId = -1;
            }
            return View(document);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(_unitOfWork.DocumentRepository.GetById(id, new[] { "Sector", "Partner", "Likvidator" }).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var doc = _entityContext.Document.Include(s => s.StatusChanges).SingleOrDefault(s => s.Id == id);
            _entityContext.Document.Remove(doc);
            _entityContext.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddCommentToDocument(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = _entityContext.Document.Where(x => x.Id == id).FirstOrDefault();

            document.Comment = comment;
            _unitOfWork.Save();

            return RedirectToAction("Details", new { id = id });
        }


        [HttpPost]
        public IActionResult ChangeStatusSentToController(int? id, int? controllerId, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (controllerId == null || controllerId == 0)
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.JNControllerSentToExecutor);
                return RedirectToAction("Details", new { id = id });
            }

            if (UserHasPermission(UserPermissions.CanSendToController))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.RecorderSentToJNController);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to forward the documents to the Executors.");
            }

            return RedirectToAction("Details", new { id = id });
        }


        [HttpPost]
        public IActionResult ChangeStatusSentToExecutor(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (UserHasPermission(UserPermissions.CanSendToExecutor))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.JNControllerSentToExecutor);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to forward the documents to the Executors.");
            }

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult ChangeStatusExecutorReceived(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (UserHasPermission(UserPermissions.CanConfirmReceiving))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.ExecutorReceivedFromJNController);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to confirm that you received the document.");
            }

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult ChangeStatusReturnedToRecorder(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = HttpContext.User as ClaimsPrincipal;
            var currentUserRole = currentUser.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value.ToInt();

            if (UserHasPermission(UserPermissions.CanReturnToRecorder))
            {
                ChangeDocumentStatus(id, comment, currentUserRole == UserRoles.JNController ? 
                    DocumentStatus.JNControllerReturnedToRecorder : 
                    DocumentStatus.ExecutorReturnedToRecorder);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to forward the documents to the Executors.");
            }

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult ChangeStatusForwardToSector(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (UserHasPermission(UserPermissions.CanSendToSectorInCharge))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.ExecutorSentToSector);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to forward the documents to sector.");
            }

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult ChangeStatusSectorProcessed(int? id, string comment, int sectorApproval)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (UserHasPermission(UserPermissions.CanSignAndVerifyDocuments))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.SectorSignedAndApproved);

                var document = _unitOfWork.DocumentRepository.GetById(id);
                document.SectorApproved = Convert.ToByte(sectorApproval);

                _unitOfWork.DocumentRepository.Update(document);
                _unitOfWork.Save();
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to process documents.");
            }

            return RedirectToAction("Details", new { id = id });
        }


        [HttpPost]
        public IActionResult ChangeStatusReturnedToController(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (UserHasPermission(UserPermissions.CanReturnToController))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.ExecutorReturnedToJNController);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to forward the documents to sector.");
            }

            return RedirectToAction("Details", new { id = id });
        }
        
        //[HttpPost]
        //public IActionResult ChangeStatusReferentProcessed(int? id, string comment, int sectorApproval)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    if (UserHasPermission(UserPermissions.CanVerifyDocuments))
        //    {
        //        ChangeDocumentStatus(id, comment, DocumentStatus.ReferentProcessed);

        //        var document = _unitOfWork.DocumentRepository.GetById(id);
        //        document.SectorApproved = Convert.ToByte(sectorApproval);

        //        _unitOfWork.DocumentRepository.Update(document);
        //        _unitOfWork.Save();
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to process documents.");
        //    }

        //    return RedirectToAction("Details", new { id = id });
        //}

        [HttpPost]
        public IActionResult ChangeStatusApproved(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (UserHasPermission(UserPermissions.CanApproveDocuments))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.Approved);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to approve documents.");
            }

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public IActionResult ChangeStatusBackToContractor(int? id, string comment)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (UserHasPermission(UserPermissions.CanReturnToSupplier))
            {
                ChangeDocumentStatus(id, comment, DocumentStatus.ReturnedToSupplier);
            }
            else
            {
                ModelState.AddModelError("NTEDoc.DataRepository.Models.Document", "Error! You don't have the permission to approve documents.");
            }

            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult LoadData([FromBody] DataTableParams dataTableParams)
        {
            var user = GetUserFromToken();
            var sektor = _unitOfWork.SectorRepository.GetById(user.OrganisationUnitId.ToInt());

            try
            {
                var recordsTotal = 0;
                var myData = GetAllDocumentsQueryable(dataTableParams, out recordsTotal).ToArray();
                var draw = dataTableParams.Draw;

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

        [HttpGet]
        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult GenerateReport([FromQuery] string reportType, [FromQuery] DataTableParams documentFilters)
        {
            if (reportType.ToLower() == "pdf")
            {
                HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);

                var recordsTotal = 0;
                var documents = GetAllDocumentsQueryable(documentFilters, out recordsTotal).ToArray();

                return View("ExportViews/ExportDocuments", documents);
            }

            return BadRequest();
        }

        private IQueryable<DocumentLight> GetAllDocumentsQueryable(DataTableParams dataTableParams, out int recordsTotal)
        {
            var user = GetUserFromToken();
            var sektor = _unitOfWork.SectorRepository.GetById(user.OrganisationUnitId.ToInt());

            // Skiping number of Rows count
            //var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var start = dataTableParams.Start;
            // Paging Length 10,20
            //var length = HttpContext.Request.Form["length"].FirstOrDefault();
            var length = dataTableParams.Length;
            // Sort Column Name
            //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

            var sortColumn = "";
            var sortColumnDirection = "";

            if (dataTableParams.Order != null && dataTableParams.Columns != null)
            {
                sortColumn = dataTableParams.Columns[dataTableParams.Order[0].Column].Name;
                sortColumnDirection = dataTableParams.Order[0].Dir;
            }

            // Sort Column Direction ( asc ,desc)
            //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)
            //var searchValue = Request.Form["search[value]"].FirstOrDefault(); 

            var searchValue = "";
            if (dataTableParams.Search != null)
            {
                searchValue = dataTableParams.Search.Value;
            }

            // Custom filters
            var year = dataTableParams.SearchYear;

            var eStatus = dataTableParams.SearchStatus;

            var dateFrom = dataTableParams.DateFrom;

            var dateTo = dataTableParams.DateTo;

            var contractType = dataTableParams.ContractType;

            var partners = dataTableParams.Partners == null ? null : dataTableParams.Partners.ToList();
            var executors = dataTableParams.Executors == null ? null : dataTableParams.Executors.ToList();
            var sectors = dataTableParams.Sectors == null ? null : dataTableParams.Sectors.ToList();

            // Advanced filters
            var recorders = dataTableParams.AdvancedFilters.Recorders.ToList();
            var createdAtFilter = dataTableParams.AdvancedFilters.CreatedAtDate;
            var documentDateFilter = dataTableParams.AdvancedFilters.DocumentDate;
            var status = dataTableParams.AdvancedFilters.Status;
            var documentType = dataTableParams.AdvancedFilters.DocumentType;
            var amount = dataTableParams.AdvancedFilters.Amount;
            var deliveryType = dataTableParams.AdvancedFilters.DeliveryType;


            if (createdAtFilter == null)
            {
                createdAtFilter = new DateRange();
            }
            if (documentDateFilter == null)
            {
                createdAtFilter = new DateRange();
            }

            if (amount == null)
            {
                amount = new AmountRange { From = 0, To = 0 };
            }

            //Paging Size (10,20,50,100,500)
            int pageSize = length != 0 ? Convert.ToInt32(length) : 10;
            int skip = start != 0 ? Convert.ToInt32(start) : 0;

            // Getting all Customer data
            var customerData = _unitOfWork.DocumentRepository.GetAll(new[] { "Sector", "DocumentType", "Partner", "Likvidator", "DocumentFiles", "DeliveryType" }) as IQueryable<Document>;

            customerData = sektor != null ? customerData.Where(s => s.SectorId == sektor.Id) : customerData;

            //var howbre = customerData.ToList();

            if (!string.IsNullOrEmpty(year))
            {
                customerData = customerData.Where(m => m.CreatedDate.Value.Year == Convert.ToInt32(year));
            }

            //Document.EStatus status = (Document.EStatus)Enum.Parse(typeof(Document.EStatus), eStatus);

            if (!string.IsNullOrEmpty(eStatus))
            {
                customerData = customerData.Where(s => s.Status.Id == Convert.ToInt32(eStatus));
            }


            //Sorting
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                // Some sorting options do not work properly. This is a handler to fix them
                var sortColumnFinal = "";
                sortColumnFinal = sortColumn switch
                {
                    "ContractNumber" => "CompanyContract.ArchiveNumber",
                    _ => sortColumn,
                };
                customerData = customerData.OrderBy(sortColumnFinal + " " + sortColumnDirection);
            }
            //Search
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.Name.Contains(searchValue)
                || m.DocumentNumber.Contains(searchValue)
                || m.Partner.naziv.Contains(searchValue)
                || m.DocumentType.Naziv.Contains(searchValue)
                || m.Sector.Naziv.Contains(searchValue)
                || m.CompanyContract.ArchiveNumber.Contains(searchValue)
                || m.CompanyContract.Name.Contains(searchValue)
                );
            }

            if (!String.IsNullOrEmpty(dateFrom))
            {
                var date = Convert.ToDateTime(dateFrom);
                customerData = customerData.Where(d => d.ReceivedDate >= date);
            }

            if (!String.IsNullOrEmpty(dateTo))
            {
                var date = Convert.ToDateTime(dateTo);
                customerData = customerData.Where(d => d.ReceivedDate <= date);
            }

            // Show all filter

            var userRoleFilter = dataTableParams.ShowAll;

            if (userRoleFilter.ForTypeId == UserRoles.Executor && !userRoleFilter.IsOn)
            {
                customerData = customerData.Where(d => d.LikvidatorId == user.UserId);
            }
            else if (userRoleFilter.ForTypeId == UserRoles.Recorder && !userRoleFilter.IsOn)
            {
                customerData = customerData.Where(d => d.CreatedByUserId == user.UserId);
            }

            // Is awaiting user action filter

            var awaitingUserFilter = dataTableParams.AwaitingUser;

            if (awaitingUserFilter.ForTypeId == UserRoles.Recorder && awaitingUserFilter.IsOn)
            {
                customerData = customerData.Where(x => DocumentStatus.RecorderStatuses.Any(xx => xx == x.StatusId.Value));
            } 
            else if (awaitingUserFilter.ForTypeId == UserRoles.JNController && awaitingUserFilter.IsOn)
            {
                customerData = customerData.Where(x => DocumentStatus.JNControllerStatuses.Any(xx => xx == x.StatusId.Value));
            } 
            else if (awaitingUserFilter.ForTypeId == UserRoles.Executor && awaitingUserFilter.IsOn)
            {
                customerData = customerData.Where(x => DocumentStatus.ExecutorStatuses.Any(xx => xx == x.StatusId.Value));
            }
            else if (awaitingUserFilter.ForTypeId == UserRoles.SectorRecorder && awaitingUserFilter.IsOn)
            {
                customerData = customerData.Where(x => DocumentStatus.SectorStatuses.Any(xx => xx == x.StatusId.Value));
            }

            // Get only longer than 48 hours on same status

            var longerThanFilter = dataTableParams.LongerThan48;

            if (longerThanFilter.IsOn)
            {
                customerData = customerData.Where(x => x.LastStatusChangeDate.HasValue ? x.LastStatusChangeDate.Value.AddHours(48) <= DateTime.Now : true);
            }

            // GOTTA TEST SOMTH

            var testNames = customerData.Select(x => x.Name).ToList();
            var testAmounts = customerData.Select(x => x.Amount).ToList();

            // YEAG TEST



            // Advaced filters dates
            if (!String.IsNullOrEmpty(createdAtFilter.From))
            {
                var date = Convert.ToDateTime(createdAtFilter.From);

                customerData = customerData.Where(d => d.CreatedDate >= date);
            }

            if (!String.IsNullOrEmpty(createdAtFilter.To))
            {
                var date = Convert.ToDateTime(createdAtFilter.To);

                customerData = customerData.Where(d => d.CreatedDate <= date);
            }

            if (!String.IsNullOrEmpty(documentDateFilter.From))
            {
                var date = Convert.ToDateTime(documentDateFilter.From);

                customerData = customerData.Where(d => d.CurrencyDate >= date);
            }

            if (!String.IsNullOrEmpty(documentDateFilter.To))
            {
                var date = Convert.ToDateTime(documentDateFilter.To);

                customerData = customerData.Where(d => d.CurrencyDate <= date);
            }


            if (contractType == 1)
            {
                customerData = customerData.Where(d => d.CompanyContractId != null);
            }
            else if (contractType == 2)
            {
                customerData = customerData.Where(d => d.CompanyContractId == null);
            }

            if (partners != null && partners.Count() > 0)
            {
                customerData = customerData.Where(d => partners.Contains(d.PartnerId));
            }

            if (executors != null && executors.Count() > 0)
            {
                customerData = customerData.Where(d => executors.Contains(d.LikvidatorId));
            }

            if (sectors != null && sectors.Count() > 0)
            {
                customerData = customerData.Where(d => sectors.Contains(d.SectorId));
            }

            if (sectors != null && recorders.Count() > 0)
            {
                customerData = customerData.Where(d => recorders.Contains(d.CreatedByUserId));
            }

            if (status != -1)
            {
                customerData = customerData.Where(d => d.StatusId == status);
            }

            if (documentType != -1)
            {
                customerData = customerData.Where(d => d.DocumentTypeId == documentType);
            }

            if (amount.From != 0)
            {
                customerData = customerData.Where(d => d.Amount >= Convert.ToDecimal(amount.From));
            }

            if (amount.To != 0)
            {
                customerData = customerData.Where(d => d.Amount <= Convert.ToDecimal(amount.To));
            }

            if (deliveryType != -1)
            {
                customerData = customerData.Where(d => d.DeliveryTypeId == deliveryType);
            }

            //total number of rows count 
            recordsTotal = customerData.Count();

            //Paging 
            var myDataInitial = customerData;
            if ((string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)) || sortColumn == "Id")
            {
                myDataInitial = myDataInitial.OrderByDescending(x => x.CreatedDate).ThenByDescending(x => x.StatusId == 3).ThenBy(x => x.LastStatusChangeDate.Value.Date);
            }

            IQueryable<DocumentLight> myData = null;

            myData = myDataInitial.Skip(skip).Take(pageSize).Select(s => new DocumentLight
            {
                Id = s.Id,
                Name = s.Name,
                DocumentType = s.DocumentType.Naziv,
                DocumentFileName = s.DocumentFiles.Where(f => f.IsPrimaryFile == 1).SingleOrDefault().FileName,
                DocumentNumber = s.DocumentNumber,
                Sector = s.Sector.Naziv,
                Partner = s.Partner.naziv,
                StatusId = s.Status.Id,
                StatusName = s.Status.Name,
                ReceivedDate = s.ReceivedDate.ToString().Substring(8, 2) + s.ReceivedDate.ToString().Substring(4, 4) + s.ReceivedDate.ToString().Substring(0, 4),
                CurrencyDate = s.CurrencyDate.ToString().Substring(8, 2) + s.CurrencyDate.ToString().Substring(4, 4) + s.CurrencyDate.ToString().Substring(0, 4),
                ContractNumber = s.CompanyContract.ArchiveNumber,
                CreatedByUser = s.CreatedByUserId,
                ExecutorId = s.LikvidatorId,
                SectorApproved = s.SectorApproved,
                LastStatusChangeDate = s.LastStatusChangeDate.Value.ToString("dd-MM-yyyy"),
                Comment = s.Comment
            }) as IQueryable<DocumentLight>;

            return myData;
        }

        private void ChangeDocumentStatus(int? id, string comment, int newStatus)
        {
            var document = _unitOfWork.DocumentRepository.GetById(id);

            document.StatusId = newStatus;
            document.LastStatusChangeDate = DateTime.Now;

            _unitOfWork.DocumentRepository.Update(document);

            var user = _dataContext.Users.Where(u => u.Username == HttpContext.User.Identity.Name).FirstOrDefault();

            StatusChange log = new StatusChange()
            {
                StatusId = newStatus,
                CreatedByUserId = user.UserId,
                CreatedDate = DateTime.Now,
                DocumentId = id,
                Comment = comment
            };

            _unitOfWork.LogRepository.InsertLog(log);
            _unitOfWork.Save();
        }

        private User GetUserFromToken()
        {
            var currentUser = HttpContext.User as ClaimsPrincipal;
            var currentUserUsername = currentUser.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            var user = _dataContext.Users.Where(u => u.Username == currentUserUsername).FirstOrDefault();

            return user;
        }

        private bool UserHasPermission(int permission)
        {
            var currentUser = HttpContext.User as ClaimsPrincipal;
            var currentUserRole = currentUser.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value.ToInt();

            var hasPermission = _entityContext.RolePermissions.Where(x => x.RoleId == currentUserRole && x.PermissionId == permission).SingleOrDefault();

            return hasPermission != null && hasPermission.Active == 1;
        }

        [AllowAnonymous]
        public FileStreamResult GetPDF(string path)
        {
            FileStream fs = new FileStream(_configuration["FilePath:FolderLocation"] + path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }


        private string FormatDate(DateTime date)
        {
            return date.Day + "-" + date.Month + "-" + date.Year;
        }
    }
}