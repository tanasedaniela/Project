using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Domain.Entities
{
    public class Medicine
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Prospect { get; set; }
        
    }
}
