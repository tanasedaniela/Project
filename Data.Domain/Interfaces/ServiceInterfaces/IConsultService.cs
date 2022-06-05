using Data.Domain.Entities;
using Data.Domain.Interfaces.ServiceInterfaces.Models.ConsultViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Data.Domain.Interfaces.ServiceInterfaces
{
    public interface IConsultService
    {
        List<Consult> GetAllConsults();
        List<Consult> GetAllConsultsForGivenMedicId(Guid medicId);
        List<Consult> GetAllConsultsForGivenPacientId(Guid pacientId);
        Consult GetConsultById(Guid id);
        Task Create(ConsultCreateModel consultCreateModel);
        Task Edit(ConsultEditModel consultEditModel);
        void Delete(Consult consult);
        bool Exists(Guid id);
        List<string> GetNamesOfFiles(Guid id);
        Stream SearchConsultFile(Guid consultId, string fileName);
        void DeleteFile(string fileName, Guid consultId);
        void DeleteFilesForGivenId(Guid id);
        string getThisFileLocation(Guid id);

        int GetNumberOfPagesForConsults();
        List<Consult> Get5ConsultsByIndex(int index);

        int GetNumberOfPagesForMyConsultsById(Guid medicId);
        List<Consult> Get5ConsultsForDoctorByIndex(Guid medicId,int index);
        int GetNumberOfPagesForMyResultsById(Guid pacientId);
        List<Consult> Get5ConsultsForPacientByIndex(Guid pacientId, int index);

        List<string> GetNamesOfModels();
        Stream SearchConsultModelFile(string fileName);

    }
}
