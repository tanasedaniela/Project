using Data.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces
{
    public interface IMedicineRepository
    {
        List<Medicine> GetAllMedicines();
        Medicine GetMedicineById(Guid id);
        Medicine GetMedicineByName(string name);
        void Create(Medicine medicine);
        void Edit(Medicine medicine);
        void Delete(Medicine medicine);
        bool Exists(Guid id);
        int GetNumberOfPagesForMedicines();
        List<Medicine> Get5MedicinesByIndex(int index);
        int GetMaxNumberOfPages();
    }
}
