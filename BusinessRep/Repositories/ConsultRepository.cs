using Data.Domain.Entities;
using Data.Domain.Interfaces;
using Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessRep.Repositories
{
    public class ConsultRepository : IConsultRepository
    {
        private readonly DatabaseContext _databaseService;

        public ConsultRepository(DatabaseContext databaseService)
        {
            _databaseService = databaseService;
        }
        
        public List<Consult> GetAllConsults()
        {

            List<Consult> rez = _databaseService.Consults.OrderBy(x => x.Id).ToList();

            foreach (Consult x in rez)
            {
                x.Result = x.Result.Decrypt();

                x.Prescription = x.Prescription.Decrypt();
            }

            return rez.OrderByDescending(x => x.Created_Date).ToList();
        }


        public List<Consult> GetAllConsultsForGivenMedicId(Guid medicId)
        {
            List<Consult> rez = _databaseService.Consults.Where(consult => consult.Doctor_Id == medicId).OrderBy(x => x.Patient_Id).ToList();

            foreach (Consult x in rez)
            {
                x.Result = x.Result.Decrypt();

                x.Prescription = x.Prescription.Decrypt();
            }

            return rez;
        }


        public List<Consult> GetAllConsultsForGivenPacientId(Guid pacientId)
        {
            List<Consult> rez = _databaseService.Consults.Where(consult => consult.Patient_Id == pacientId).OrderBy(x => x.Doctor_Id).ToList();

            foreach(Consult x in rez)
            {
                x.Result = x.Result.Decrypt();

                x.Prescription = x.Prescription.Decrypt();
            }

            return rez;
        }


        public Consult GetConsultById(Guid id)
        {
            Consult rez = _databaseService.Consults.SingleOrDefault(consult => consult.Id == id);

            rez.Result = rez.Result.Decrypt();

            rez.Prescription = rez.Prescription.Decrypt();

            return rez;
        }
        

        public void Create(Consult consult)
        {
            consult.Result=consult.Result.Encrypt();

            consult.Prescription=consult.Prescription.Encrypt();


            _databaseService.Consults.Add(consult);

            _databaseService.SaveChanges();
        }


        public void Edit(Consult consult)
        {
            consult.Result=consult.Result.Encrypt();

            consult.Prescription=consult.Prescription.Encrypt();

            _databaseService.Consults.Update(consult);

            _databaseService.SaveChanges();
        }


        public void Delete(Consult consult)
        {
            _databaseService.Consults.Remove(consult);

            _databaseService.SaveChanges();
        }

        public bool Exists(Guid id)
        {
            return _databaseService.Consults.Any(e => e.Id == id);
        }

        public int GetNumberOfConsults()
        {
            return _databaseService.Consults.Count();
        }

        public int GetNumberOfConsultsForMedic(Guid medicId)
        {
            return _databaseService.Consults.Where(consult => consult.Doctor_Id == medicId).Count();
        }

        public int GetNumberOfConsultsForPacient(Guid pacientId)
        {
            return _databaseService.Consults.Where(consult => consult.Patient_Id == pacientId).Count();
        }
    }
}
