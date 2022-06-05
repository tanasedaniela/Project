using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces.ServiceInterfaces.Models.ConsultViewModels
{
    public class ConsultEditModel
    {
        public ConsultEditModel()
        {
            // EF
        }

        public Guid Id { get; set; }

        public Guid MedicId { get; set; }

        public Guid PacientId { get; set; }

        public DateTime ConsultDate { get; set; }

        //An list of Medicine Names: Ex: Med1(indicatiiMed1), Med2
        public string Medicines { get; set; }

        public string ConsultResult { get; set; }

        public IEnumerable<IFormFile> File { get; set; }
        
        public ConsultEditModel(Guid id, Guid medicId, Guid pacientId, DateTime consultDate, string medicines, string consultResult)
        {
            Id = id;
            MedicId = medicId;
            PacientId = pacientId;
            ConsultDate = consultDate;
            Medicines = medicines;
            ConsultResult = consultResult;
        }

    }
}
