using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessRep.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly DatabaseContext _databaseContext;

        public MedicineRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public List<Medicine> GetAllMedicines()
        {

            List<Medicine> rez = _databaseContext.Medicines.ToList();

            foreach(Medicine x in rez)
            {

                x.Name = x.Name.Decrypt();

                x.Prospect = x.Prospect.Decrypt();

            }

            return rez.OrderBy(x => x.Name).ToList();
        }


        public Medicine GetMedicineById(Guid id)
        {
            Medicine rez = _databaseContext.Medicines.SingleOrDefault(medicine => medicine.Id == id);

            rez.Name = rez.Name.Decrypt();

            rez.Prospect = rez.Prospect.Decrypt();

            return rez;
        }


        public Medicine GetMedicineByName(string name)
        {
            Medicine rez = _databaseContext.Medicines.SingleOrDefault(medicine => medicine.Name.Decrypt().ToLower()== name.ToLower());

           // rez.Name = rez.Name.Decrypt();

            //rez.Prospect = rez.Prospect.Decrypt();

            return rez;
        }


        public void Create(Medicine medicine)
        {
            medicine.Name = medicine.Name.Encrypt();

            medicine.Prospect = medicine.Prospect.Encrypt();

            _databaseContext.Medicines.Add(medicine);

            _databaseContext.SaveChanges();
        }


        public void Edit(Medicine medicine)
        {
            medicine.Name = medicine.Name.Encrypt();

            medicine.Prospect = medicine.Prospect.Encrypt();

            _databaseContext.Medicines.Update(medicine);

            _databaseContext.SaveChanges();
        }


        public void Delete(Medicine medicine)
        {
            _databaseContext.Medicines.Remove(medicine);

            _databaseContext.SaveChanges();
        }
        
        public bool Exists(Guid id)
        {
            return _databaseContext.Medicines.Any(e => e.Id == id);
        }

        public int GetNumberOfPagesForMedicines()
        {
            int rez = 0;
            int count = _databaseContext.Medicines.Count();
            rez = count / 5;

            if (rez * 5 < count)
            {
                rez++;
            }

            return rez;
        }

        public List<Medicine> Get5MedicinesByIndex(int index)
        {
            List<Medicine> rez = new List<Medicine>() { };
            List<Medicine> allMedicines = GetAllMedicines();
            int start = (index - 1) * 5;
            int finish = start + 5;

            for (int i = start; i < finish; i++)
            {
                if (i < allMedicines.Count)
                {
                    rez.Add(allMedicines[i]);
                }
            }

            return rez;
        }

        public int GetMaxNumberOfPages()
        {
            return _databaseContext.Medicines.Count();
        }
    }
}
