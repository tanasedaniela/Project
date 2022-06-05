using System;

namespace MediArch.Models.AccountViewModels
{
    public class UserRecordViewModel
    {
        public UserRecordViewModel()
        {
            // EF
        }

        
        public string Id { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Title { get; set; }

        public string CabinetAddress { get; set; }

        public bool ActiveAccount { get; set; }

        public DateTime CreatedDate { get; set; }

        public UserRecordViewModel(string id, string role,string email, string firstName, string lastName, DateTime birthDate, string title, string cabinetAdress, 
            string phoneNumber, bool activeAccount, DateTime createdDate)
        {
            Id = id;
            Role = role;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            Title = title;
            CabinetAddress = cabinetAdress;
            ActiveAccount = activeAccount;
            CreatedDate = createdDate;
        }
    }
}
