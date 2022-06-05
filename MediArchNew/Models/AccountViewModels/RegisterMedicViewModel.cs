using FluentValidation.Attributes;
using MediArch.Models.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace MediArch.Models.AccountViewModels
{
    //[Validator(typeof(RegisterMedicViewModelValidator))]
    [Validator(typeof(UserValidation))]
    public class RegisterMedicViewModel
    {

        public RegisterMedicViewModel() // MVC can call this
        {
        }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required!")]
        [Display(Name = "First Name")]
        [RegularExpression(@"[A-Za-z]{2,}([\s|-]{1}[A-Za-z]{2,}){0,}", ErrorMessage = "Format not respected.")] //ăîșțâĂÎȘȚÂ
        [MinLength(2, ErrorMessage = "Each name mush have at least 2 characters!")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"[A-Za-z]{2,}([\s|-]{1}[A-Za-z]{2,}){0,}", ErrorMessage = "Format not respected.")]
        [MinLength(2, ErrorMessage="Each name mush have at least 2 characters!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "BirthDate is required!")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail is required!")]
        [Display(Name = "Your Email")]
        //[RegularExpression(@"(([A-Za-z0-9_|-]{3,})(@)((gmail)|(yahoo)){1}(.)(([A-Za-z]+)))", ErrorMessage = "Format not respected.")]
        [RegularExpression(@"(([^@]{3,})(@)((gmail)|(yahoo)){1}(.)(([A-Za-z]+)))", ErrorMessage = "Format not respected.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [Display(Name = "Phone Number")]
        [DataType("PhoneNumber", ErrorMessage = "Format not allowed.")]
        [RegularExpression(@"([0-9]){10}", ErrorMessage = "Format not respected.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        [Display(Name = "Title")]
        [RegularExpression(@"[A-Za-z|.]{2,}([\s|-|,]{1,2}[A-Za-z|.]{2,}){0,}", ErrorMessage = "Format not respected.")]
        [MinLength(3, ErrorMessage = "Title must have at least 3 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Your Cabinet Adress is required!")]
        [Display(Name = "Cabinet Adress")]
        [RegularExpression(@"[A-Za-z0-9|.|-]{1,}([\s|-|,]{1,2}[A-Za-z0-9|.|-]{1,}){0,}", ErrorMessage = "Format not respected.")]
        [MinLength(1, ErrorMessage = "Adress must have at least 1 character")]
        public string CabinetAddress { get; set; }
    }
    
}
