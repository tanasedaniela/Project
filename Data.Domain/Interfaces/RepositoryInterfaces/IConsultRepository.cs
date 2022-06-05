using Data.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces
{
    public interface IConsultRepository
    {
        List<Consult> GetAllConsults();
        List<Consult> GetAllConsultsForGivenMedicId(Guid medicId);
        List<Consult> GetAllConsultsForGivenPacientId(Guid pacientId);

        int GetNumberOfConsults();
        int GetNumberOfConsultsForMedic(Guid medicId);
        int GetNumberOfConsultsForPacient(Guid pacientId);

        Consult GetConsultById(Guid id);
        void Create(Consult consult);
        void Edit(Consult consult);
        void Delete(Consult consult);
        bool Exists(Guid id);
    }
}
