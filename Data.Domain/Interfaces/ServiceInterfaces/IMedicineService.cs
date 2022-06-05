using Data.Domain.Entities;
using Data.Domain.Interfaces.ServiceInterfaces.Models.MedicineViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces.ServiceInterfaces
{
    public interface IMedicineService
    {
        List<Medicine> GetAllMedicines();
        Medicine GetMedicineById(Guid id);
        Medicine GetMedicineByName(string name);
        Task Create(MedicineCreateModel medicine);
        Task Edit(MedicineEditModel medicine);
        void Delete(Medicine medicine);
        bool Exists(Guid id);
        int GetNumberOfPagesForMedicines();
        List<Medicine> Get5MedicinesByIndex(int index);
        List<Medicine> SearchMedicinesByName(string text);
        int GetMaxNumberOfPAges();

        List<string> GetNamesOfFiles(Guid MedicineId);
        Stream SearchMedicineFile(Guid MedicineId, string fileName);
        void DeleteFile(Guid MedicineId, string fileName);
        void DeleteFilesForGivenId(Guid id);
        string Simplify(string prospect);
    }
}
