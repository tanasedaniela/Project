using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MediArch.Models;
using Data.Domain.Interfaces.ServiceInterfaces;
using Data.Domain.Interfaces.ServiceInterfaces.Models.ConsultViewModels;
using MediArch.Services;
using MediArch.Services.Interfaces;

namespace MediArch.Controllers
{
    [Authorize]
    public class ConsultsController : Controller
    {
        
        private readonly IConsultService _service;

        
        private readonly IApplicationUserService _user_service;
        
        public ConsultsController(IConsultService service,
           
            IApplicationUserService user_service)
        {

            _service = service;
           
            _user_service = user_service;
        }

        // GET: Consults
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Index()
        {
            return View(_service.GetAllConsults());
        }

        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult ConsultsPaginated(int noPage)
        {

            if (noPage < 1)
            {
                noPage = 1;
            }

            if (noPage > _service.GetNumberOfPagesForConsults())
            {
                noPage = _service.GetNumberOfPagesForConsults();
            }

            return View(_service.Get5ConsultsByIndex(noPage));
        }

        // GET: Consults
        [Authorize(Roles = "Medic")]
        public IActionResult MyConsults(string id)
        {
            Guid medicId = new Guid(id);
            return View(_service.GetAllConsultsForGivenMedicId(medicId));
        }

        
        
        [Authorize(Roles = "Pacient")]
        public IActionResult MyResults(string id)
        {
            Guid pacientId = new Guid(id);
            return View(_service.GetAllConsultsForGivenPacientId(pacientId));
        }


        [Authorize(Roles = "Medic")]
        public IActionResult MyConsultsPaginated(string id, int NoPage)
        {
            ViewData["Id"] = id;
            Guid medicId = new Guid(id);
            return View(_service.Get5ConsultsForDoctorByIndex(medicId, NoPage));
        }

        [Authorize(Roles = "Pacient")]
        public IActionResult MyResultsPaginated(string id, int NoPage)
        {
            ViewData["Id"] = id;
            Guid pacientId = new Guid(id);
            return View(_service.Get5ConsultsForPacientByIndex(pacientId, NoPage));
        }


        // GET: Consults/Details/5
        [Authorize(Roles = "Owner, Moderator, Medic, Pacient")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                /*return NotFound();*/
                return RedirectToAction("Not_Found", "Home");
            }
            
            var consult = _service.GetConsultById(id.Value);
            if (consult == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(consult);
        }

        // GET: Consults/Create
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Consults/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator")]
        public async Task<IActionResult> Create([Bind("MedicId,PacientId,Medicines,ConsultResult,File")] ConsultCreateModel consultCreateModel)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(consultCreateModel);
                return RedirectToAction(nameof(ConsultsPaginated));
            }
            return View(consultCreateModel);
        }


        // GET: Consults/Create
        [Authorize(Roles = "Owner, Moderator, Medic")]
        public IActionResult CreateNewConsult(Guid? medicId, Guid? pacientId)
        {
            TempData["MId"] = medicId.Value.ToString();
            TempData["PId"] = pacientId.Value.ToString();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator, Medic")]
        public async Task<IActionResult> CreateNewConsult(Guid? medicId, Guid? pacientId, [Bind("MedicId,PacientId,Medicines,ConsultResult,File")] ConsultCreateModel consultCreateModel)
        {
            await _service.Create(consultCreateModel);

            ApplicationUser Patient = _user_service.GetUserById(pacientId.ToString());
            ApplicationUser Doctor = _user_service.GetUserById(medicId.ToString());

            /****** This must be uncommended ******/
            

            return RedirectToAction("Index","Home");
        }

        // GET: Consults/Edit/5
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            
            var consult = _service.GetConsultById(id.Value);

            if (consult == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            ConsultEditModel consultEditModel = new ConsultEditModel(
            
                consult.Id,
                consult.Doctor_Id,
                consult.Patient_Id,
                consult.Created_Date,
                consult.Prescription,
                consult.Result
            );
            return View(consultEditModel);
        }

        // POST: Consults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        // This edit would be for admins
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MedicId,PacientId,ConsultDate,Medicines,ConsultResult,File")] ConsultEditModel consultEditModel)
        {
            if (id != consultEditModel.Id)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Edit(consultEditModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultExists(consultEditModel.Id))
                    {
                        return RedirectToAction("Not_Found", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(consultEditModel);
        }

        // GET: Consults/Delete/5
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var consult = _service.GetConsultById(id.Value);

            if (consult == null)
            {
                return RedirectToAction("Not_Found", "Home"); ;
            }

            return View(consult);
        }

        // POST: Consults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult DeleteConfirmed(Guid id)
        {

            var consult = _service.GetConsultById(id);
            _service.Delete(consult);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult Download(Guid consultId, string fileName)
        {
            var file = _service.SearchConsultFile(consultId, fileName);

            return File(file, "application/octet-stream", fileName);
        }

        [HttpPost]
        public IActionResult DownloadModel(string fileName)
        {
            var file = _service.SearchConsultModelFile(fileName);

            return File(file, "application/octet-stream", fileName);
        }

        [HttpDelete] 
        public IActionResult DeleteFile(Guid consultId, string fileName)
        {
            _service.DeleteFile(fileName, consultId);

            return RedirectToAction("Delete", "Consults", new { id = consultId });
        }

        private bool ConsultExists(Guid id)
        {
            return _service.Exists(id);
        }
    }
}
