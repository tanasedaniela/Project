using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Data.Domain.Interfaces.ServiceInterfaces.Models.MedicineViewModels
{
    public class MedicineCreateModel
    {
        public string Name { get; set; }

        public string Prospect { get; set; }

        public IEnumerable<IFormFile> File { get; set; }
    }
}
