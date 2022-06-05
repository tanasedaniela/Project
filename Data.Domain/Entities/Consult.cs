using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Consult
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid Doctor_Id { get; set; }
        
        public Guid Patient_Id { get; set; }
        
        public DateTime Created_Date { get; set; }
        
        public string Prescription { get; set; }
        
        public string Result { get; set; }
        
    }
}
