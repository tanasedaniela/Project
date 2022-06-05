using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Data.Domain.Interfaces.ServiceInterfaces.Models.MedicineViewModels
{
    public class MedicineEditModel
    {
        public MedicineEditModel()
        {
            // EF
        }

        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Prospect { get; set; }

        public IEnumerable<IFormFile> File { get; set; }

        public MedicineEditModel(Guid id, string name, string prospect)
        {
            Id = id;
            Name = name;
            Prospect = prospect;
        }
    }
}
