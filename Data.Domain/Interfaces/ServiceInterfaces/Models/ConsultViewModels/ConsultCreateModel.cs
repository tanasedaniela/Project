using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces.ServiceInterfaces.Models.ConsultViewModels
{
    public class ConsultCreateModel
    {
        public Guid MedicId { get; set; }
        
        public Guid PacientId { get; set; }
        
        //An list of Medicine Names: Ex: Med1(indicatiiMed1), Med2
        public string Medicines { get; set; }

        public string ConsultResult { get; set; }

        public IEnumerable<IFormFile> File { get; set; }
    }
}
