using Data.Domain.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Data.Domain.Interfaces.ServiceInterfaces.Models.MedicineViewModels;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessRep.Services
{
    public class MedicineService : IMedicineService
    {

        private readonly IMedicineRepository _repository;

        private readonly IHostingEnvironment _env;

        public MedicineService(IMedicineRepository repository, IHostingEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public List<Medicine> GetAllMedicines()
        {
            return _repository.GetAllMedicines();
        }

        public int GetMaxNumberOfPAges()
        {
            return _repository.GetMaxNumberOfPages();
        }

        public List<Medicine> Get5MedicinesByIndex(int index)
        {
            return _repository.Get5MedicinesByIndex(index);
        }
        
        public Medicine GetMedicineById(Guid id)
        {
            return _repository.GetMedicineById(id);
        }

        public Medicine GetMedicineByName(string name)
        {
            return _repository.GetMedicineByName(name);
        }

        public bool Exists(Guid id)
        {
            return _repository.Exists(id);
        }


        public async Task Create(MedicineCreateModel medicineCreateModel)
        {
            Medicine med = new Medicine()
            {
                Id = new Guid(),
                Name = medicineCreateModel.Name,
                Prospect = medicineCreateModel.Prospect
            };

            _repository.Create(med);

            if (medicineCreateModel.File != null)
            {
                foreach (var file in medicineCreateModel.File)
                {
                    if (file.Length > 0)
                    {
                        string path = Path.Combine(_env.WebRootPath, "Medicines\\" + med.Id);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }



        public void Delete(Medicine medicine)
        {
            DeleteFilesForGivenId(medicine.Id);

            _repository.Delete(medicine);
        }

        public void DeleteFilesForGivenId(Guid id)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Medicines/" + id);
            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }
        }


        public void DeleteFile(Guid MedicineId, string fileName)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Medicines/" + MedicineId + "/" + fileName);

            if (File.Exists(searchedPath))
            {
                File.Delete(searchedPath);
            }
        }

      
        public List<string> GetNamesOfFiles(Guid MedicineId)
        {
            List<string> fileList = new List<string>();
            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Medicines\\" + MedicineId;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var files in Directory.GetFiles(path))
            {
                fileList.Add(Path.GetFileName(files));
            }

            return fileList;
        }

        public int GetNumberOfPagesForMedicines()
        {
            return _repository.GetNumberOfPagesForMedicines();
        }
        

        public Stream SearchMedicineFile(Guid MedicineId, string fileName)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Medicines/" + MedicineId + "/" + fileName);
            Stream file = new FileStream(searchedPath, FileMode.Open);

            return file;
        }

        public async Task Edit(MedicineEditModel medicineEditModel)
        {
            Medicine medicine = new Medicine()
            {
                Id = medicineEditModel.Id,
                Name = medicineEditModel.Name,
                Prospect = medicineEditModel.Prospect
            };

            _repository.Edit(medicine);

            if (medicineEditModel.File != null)
            {
                foreach (var file in medicineEditModel.File)
                {
                    if (file.Length > 0)
                    {
                        var path = Path.Combine(_env.WebRootPath, "Medicines/" + medicine.Id);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
        }

        public List<Medicine> SearchMedicinesByName(string text)
        {
            List<Medicine> rez = GetAllMedicines().Where(x => x.Name.ToLower().Contains(text.ToLower())).ToList();

            if (rez.Count == 0)
            {
                return new List<Medicine>();
            }

            return rez;
        }

        public string Simplify(string prospect)
        {
            string rez = "";
            if (prospect != null)
            {
                if (prospect.Length > 128)
                {
                    rez = rez + prospect.Substring(0, 124);
                    rez = rez + " ...";
                }
                else
                {
                    rez = prospect;
                }
            }
            return rez;
        }
    }
}
