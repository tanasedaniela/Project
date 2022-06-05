using MediArch.Models;
using MediArch.Models.ApplicationUserViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediArch.Services.Interfaces
{
    public interface IApplicationUserService
    {
        List<ApplicationUser> GetOPUSers();
        List<ApplicationUser> GetNormalUsers();
        List<ApplicationUser> GetAllUsers();
        List<ApplicationUser> GetAllMedics();
        List<ApplicationUser> GetAllPacients();
        ApplicationUser GetUserById(string id);
        ApplicationUser GetUserByUserName(string userName);
        string GetUserIdByUserName(string userName);
        string GetFullUserNameById(string id);
        void EditApplicationUser(ApplicationUserEditModel applicationUserEditModel);
        void DeleteApplicationUser(ApplicationUser appusr);
        bool ApplicationUserExists(string id);
        void ModifyCabinetAddress(string id, string newAddress);
        string getUserFirstNameByEmail(string email);

        List<ApplicationUserViewModel> SearchUsers(string text);
        List<ApplicationUserViewModel> SearchMedics(string text);
        List<ApplicationUserViewModel> SearchPacients(string text);
        string DetermineUserRole(string id);
        List<ApplicationUser> GetMedicListByLocation(string location);
        int GetAgeOfUser(string id);
        string GetFullNameById(string id);
        List<string> GetAllSpecializations();
        List<ApplicationUser> GetAllMedicsForCertainSpecialization(string specialization);
        Task UploadProfilePicture(string id, IEnumerable<IFormFile> UploadedFile);
        void DeleteProfilePictureFilesForGivenId(Guid id);
        string GetProfilePictureLink(string id);
        void DeleteCertainFile(Guid userId, string fileName);
        string GetNameOfProfilePictureById(Guid userId);
        bool CheckIfThisUserHaveAProfilePicture(Guid id);
        int GetNumberOfPagesForAllUsers();
        IEnumerable<ApplicationUser> Get5UsersByIndex(int index);
        int GetNumberOfPagesForDoctors();
        IEnumerable<ApplicationUser> Get5DoctorsByIndex(int index);
        int GetNumberOfPagesForPacients();
        IEnumerable<ApplicationUser> Get5PacientsByIndex(int index);
        string getUrlBase();
        bool Exists(string id);

        void SetActive(string id);
        void SetInactive(string id);

        string GetActivity(string id);
    }
}
