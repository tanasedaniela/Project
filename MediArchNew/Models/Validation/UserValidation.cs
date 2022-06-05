using FluentValidation;
using MediArch.Data;
using MediArch.Models.AccountViewModels;
using System.Linq;

namespace MediArch.Models.Validation
{
    public class UserValidation : AbstractValidator<RegisterMedicViewModel>
    {
        private readonly ApplicationDbContext _databaseService;

        public UserValidation(ApplicationDbContext databaseService)
        {
            _databaseService = databaseService;
            
            RuleFor(x => x.Email).Must(BeUniqueEmail).WithMessage("This mail address was already used!");
        }
        
        private bool BeUniqueEmail(string email)
        {
            foreach (var x in _databaseService.Users.ToList())
            {
                if (x.Email.Equals(email))
                    return false;
            }
            return true;
        }
    }
}
