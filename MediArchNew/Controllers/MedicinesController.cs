using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Data.Domain.Interfaces.ServiceInterfaces.Models.MedicineViewModels;
using Data.Domain.Interfaces.ServiceInterfaces;

namespace MediArch.Controllers
{
    [Authorize]
    public class MedicinesController : Controller
    {

        private readonly IMedicineService _service;
        
        public MedicinesController(IMedicineService service)
        {
            _service = service;
        }

        public ActionResult SearchForMedicines(string text)
        {
            return Json(_service.SearchMedicinesByName(text));
        }


        // GET: Medicines
        [Authorize(Roles = "Owner, Moderator, Medic, Pacient")]
        public IActionResult Index()
        {
            return View(_service.GetAllMedicines());
        }
        
        // GET: Medicines/Details/5
        [Authorize(Roles = "Owner, Moderator, Medic, Pacient")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            
            var medicine = _service.GetMedicineById(id.Value);
            if (medicine == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(medicine);
        }
       
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
        public async Task<IActionResult> Create([Bind("Name,Prospect,File")] MedicineCreateModel medicineCreateModel)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(medicineCreateModel);
                return RedirectToAction(nameof(Index));
            }
            return View(medicineCreateModel);
        }
        

        // GET: Consults/Edit/5
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var consult = _service.GetMedicineById(id.Value);

            if (consult == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            MedicineEditModel medicineEditModel = new MedicineEditModel(

                consult.Id,
                consult.Name,
                consult.Prospect
            );
            return View(medicineEditModel);
        }

        // POST: Consults/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        // This edit would be for admins
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Prospect,File")] MedicineEditModel medicineEditModel)
        {
            if (id != medicineEditModel.Id)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.Edit(medicineEditModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.Exists(medicineEditModel.Id))
                    {
                        return RedirectToAction("Not_Found", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medicineEditModel);
        }

        // GET: Medicines/Delete/5
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            
            var medicine = _service.GetMedicineById(id.Value);

            if (medicine == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var medicine = _service.GetMedicineById(id);
            _service.Delete(medicine);

            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(Guid id)
        {
            return _service.Exists(id);
        }

        [HttpPost]
        public IActionResult Download(Guid medicineId, string fileName)
        {
            var file = _service.SearchMedicineFile(medicineId, fileName);

            return File(file, "application/octet-stream", fileName);
        }

        [HttpDelete]
        public IActionResult DeleteFile(Guid medicineId, string fileName)
        {
            _service.DeleteFile(medicineId, fileName);

            return RedirectToAction("Delete", "Medicines", new { id = medicineId });
        }

    }
}
