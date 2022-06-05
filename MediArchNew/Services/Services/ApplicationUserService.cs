using Data.Persistence;
using MediArch.Data;
using MediArch.Models;
using MediArch.Models.ApplicationUserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MediArch.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MediArch.Services.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext _context;

        private readonly DatabaseContext _databaseContext;

        private readonly IHostingEnvironment _env;

        public ApplicationUserService(ApplicationDbContext context, DatabaseContext databaseContext, IHostingEnvironment env)
        {
            _context = context;
            _databaseContext = databaseContext;
            _env = env;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            List<ApplicationUser> owners = (from appUsr in _context.ApplicationUser
                                            join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                            join role in _context.Roles on usrRoles.RoleId equals role.Id
                                            where role.Name == "Owner"
                                            select appUsr).OrderBy(x => x.Email).ToList();
            List<ApplicationUser> moderators = ( from appUsr in _context.ApplicationUser
                                                 join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                                 join role in _context.Roles on usrRoles.RoleId equals role.Id
                                                 where role.Name == "Moderator"
                                                 select appUsr).OrderBy(x => x.Email).ToList();

            List<ApplicationUser> medics = (from appUsr in _context.ApplicationUser
                                            join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                            join role in _context.Roles on usrRoles.RoleId equals role.Id
                                            where role.Name == "Medic"
                                            select appUsr).OrderBy(x => x.Email).ToList();

            List<ApplicationUser> pacients = (  from appUsr in _context.ApplicationUser
                                                join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                                join role in _context.Roles on usrRoles.RoleId equals role.Id
                                                where role.Name == "Pacient"
                                                select appUsr).OrderBy(x => x.Email).ToList();

            List<ApplicationUser> result = new List<ApplicationUser>();
            
            foreach (ApplicationUser usr in owners)
            {
                result.Add(usr);
            }
            foreach (ApplicationUser usr in moderators)
            {
                result.Add(usr);
            }
            foreach (ApplicationUser usr in medics)
            {
                result.Add(usr);
            }
            foreach (ApplicationUser usr in pacients)
            {
                result.Add(usr);
            }

            foreach (ApplicationUser usr in result)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title!="")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress!="")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

                return result;
        }

        public List<ApplicationUser> GetNormalUsers()
        {
            List<ApplicationUser> medics = (from appUsr in _context.ApplicationUser
                                            join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                            join role in _context.Roles on usrRoles.RoleId equals role.Id
                                            where role.Name == "Medic"
                                            select appUsr).OrderBy(x => x.Email).ToList();

            List<ApplicationUser> pacients = (from appUsr in _context.ApplicationUser
                                              join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                              join role in _context.Roles on usrRoles.RoleId equals role.Id
                                              where role.Name == "Pacient"
                                              select appUsr).OrderBy(x => x.Email).ToList();

            List<ApplicationUser> result = new List<ApplicationUser>();
            
            foreach (ApplicationUser usr in medics)
            {
                result.Add(usr);
            }
            foreach (ApplicationUser usr in pacients)
            {
                result.Add(usr);
            }

            foreach (ApplicationUser usr in result)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

            return result;
        }
            public List<ApplicationUser> GetOPUSers()
        {
            List<ApplicationUser> owners = (from appUsr in _context.ApplicationUser
                                            join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                            join role in _context.Roles on usrRoles.RoleId equals role.Id
                                            where role.Name == "Owner"
                                            select appUsr).OrderBy(x => x.Email).ToList();
            List<ApplicationUser> moderators = (from appUsr in _context.ApplicationUser
                                                join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                                join role in _context.Roles on usrRoles.RoleId equals role.Id
                                                where role.Name == "Moderator"
                                                select appUsr).OrderBy(x => x.Email).ToList();
            List<ApplicationUser> result = new List<ApplicationUser>();

            foreach (ApplicationUser usr in owners)
            {
                result.Add(usr);
            }
            foreach (ApplicationUser usr in moderators)
            {
                result.Add(usr);
            }

            foreach (ApplicationUser usr in result)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

            return result;
        }

        public List<ApplicationUser> GetAllMedics()
        {
            List<ApplicationUser> medics = (from appUsr in _context.ApplicationUser
                                            join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                            join role in _context.Roles on usrRoles.RoleId equals role.Id
                                            where role.Name == "Medic"
                                            select appUsr).Where(x=>x.ActiveAccount==true).OrderBy(x => x.Email).ToList();
            
            List<ApplicationUser> result = new List<ApplicationUser>();

            foreach (ApplicationUser usr in medics)
            {
                result.Add(usr);
            }

            foreach (ApplicationUser usr in result)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

            return result;

        }

        public List<ApplicationUser> GetAllPacients()
        {
            List<ApplicationUser> pacients = (from appUsr in _context.ApplicationUser
                                              join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                              join role in _context.Roles on usrRoles.RoleId equals role.Id
                                              where role.Name == "Pacient"
                                              select appUsr).Where(x => x.ActiveAccount == true).OrderBy(x => x.Email).ToList();

            List<ApplicationUser> result = new List<ApplicationUser>();

            foreach (ApplicationUser usr in pacients)
            {
                result.Add(usr);
            }

            foreach (ApplicationUser usr in result)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

            return result;

        }
        
        public List<ApplicationUserViewModel> SearchUsers(string text)
        {
            List<ApplicationUser> searchedUsers = GetNormalUsers().Where(x=>x.FirstName.ToUpper().Contains(text.ToUpper()) || 
                                                                    x.LastName.ToUpper().Contains(text.ToUpper()) || 
                                                                    x.Email.Substring(0,x.Email.IndexOf('@')).ToUpper().Contains(text.ToUpper()))
                                            .OrderBy(x => x.Email).ToList();
            List<ApplicationUserViewModel> rez = new List<ApplicationUserViewModel>();
            if (searchedUsers.Count == 0)
            {
                return rez;
            }


            foreach (ApplicationUser user in searchedUsers)
            {
                var usr = new ApplicationUserViewModel
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BirthDate = user.BirthDate,
                    Title = user.Title,
                    CabinetAddress = user.CabinetAddress
                };
                rez.Add(usr);
            }

            foreach (ApplicationUserViewModel usr in rez)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

            return rez;
        }

        public List<ApplicationUserViewModel> SearchMedics(string text)
        {
            List<ApplicationUser> searchedUsers = GetAllMedics().Where(x => x.FirstName.ToUpper().Contains(text.ToUpper()) ||
                                                               x.LastName.ToUpper().Contains(text.ToUpper()) ||
                                                               x.Email.Substring(0, x.Email.IndexOf('@')).ToUpper().Contains(text.ToUpper()))
                                           .OrderBy(x => x.Email).ToList();

            List<ApplicationUserViewModel> rez = new List<ApplicationUserViewModel>();
            if (searchedUsers.Count == 0)
            {
                return rez;
            }


            foreach (ApplicationUser user in searchedUsers)
            {
                var usr = new ApplicationUserViewModel
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BirthDate = user.BirthDate,
                    Title = user.Title,
                    CabinetAddress = user.CabinetAddress
                };
                rez.Add(usr);
            }

            foreach (ApplicationUserViewModel usr in rez)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

            return rez;
        }

        public List<ApplicationUserViewModel> SearchPacients(string text)
        {
            List<ApplicationUser> searchedUsers = GetAllPacients().Where(x => x.FirstName.ToUpper().Contains(text.ToUpper()) ||
                                                     x.LastName.ToUpper().Contains(text.ToUpper()) ||
                                                     x.Email.Substring(0, x.Email.IndexOf('@')).ToUpper().Contains(text.ToUpper()))
                                          .OrderBy(x => x.Email).ToList();

            List<ApplicationUserViewModel> rez = new List<ApplicationUserViewModel>();
            if (searchedUsers.Count == 0)
            {
                return rez;
            }


            foreach (ApplicationUser user in searchedUsers)
            {
                var usr = new ApplicationUserViewModel
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    BirthDate = user.BirthDate,
                    Title = user.Title,
                    CabinetAddress = user.CabinetAddress
                };
                rez.Add(usr);
            }

            foreach (ApplicationUserViewModel usr in rez)
            {
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }

            return rez;
        }

        public ApplicationUser GetUserById(string id)
        {
            ApplicationUser usr = _context.ApplicationUser.SingleOrDefault(m => m.Id == id);

            usr.FirstName = usr.FirstName.Decrypt();
            usr.LastName = usr.LastName.Decrypt();
            usr.PhoneNumber = usr.PhoneNumber.Decrypt();
            if (usr.Title != null || usr.Title != "")
            {
                usr.Title = usr.Title.Decrypt();
            }
            if (usr.CabinetAddress != null || usr.CabinetAddress != "")
            {
                usr.CabinetAddress = usr.CabinetAddress.Decrypt();
            }

            return usr;
        }
        public ApplicationUser GetUserByUserName(string userName)
        {
            ApplicationUser usr = _context.ApplicationUser.SingleOrDefault(m => m.UserName == userName);

            usr.FirstName = usr.FirstName.Decrypt();
            usr.LastName = usr.LastName.Decrypt();
            usr.PhoneNumber = usr.PhoneNumber.Decrypt();
            if (usr.Title != null || usr.Title != "")
            {
                usr.Title = usr.Title.Decrypt();
            }
            if (usr.CabinetAddress != null || usr.CabinetAddress != "")
            {
                usr.CabinetAddress = usr.CabinetAddress.Decrypt();
            }

            return usr;
        }

        public string GetFullUserNameById(string id)
        {
            ApplicationUser usr = _context.ApplicationUser.SingleOrDefault(m => m.Id == id);
            return usr.FirstName.Decrypt() + " " + usr.LastName.Decrypt();
        }

        public string GetUserIdByUserName(string userName)
        {
            return _context.ApplicationUser.SingleOrDefault(m => m.UserName == userName).Id.ToString();
        }

        public void ModifyCabinetAddress(string id, string newAddress)
        {
            ApplicationUser user = GetUserById(id);

            user.CabinetAddress = newAddress.Encrypt();

            _context.Update(user);

            _context.SaveChanges();
        }

        public void EditApplicationUser(ApplicationUserEditModel applicationUserEditModel)
        {
            
            ApplicationUser user = GetUserById(applicationUserEditModel.Id);
            
            user.FirstName = applicationUserEditModel.FirstName.Encrypt();
                
            user.LastName = applicationUserEditModel.LastName.Encrypt();
               
            user.BirthDate = applicationUserEditModel.BirthDate;

            if (DetermineUserRole(applicationUserEditModel.Id).ToUpper() == "MEDIC")
            {
                if (applicationUserEditModel.Title != null)
                {
                    user.Title = applicationUserEditModel.Title.Encrypt();
                }
                if (applicationUserEditModel.CabinetAddress != null)
                {
                    user.CabinetAddress = applicationUserEditModel.CabinetAddress.Encrypt();
                }
            }
            user.PhoneNumber = applicationUserEditModel.PhoneNumber.Encrypt();
                

            // user.Email = applicationUserEditModel.Email;
            // user.UserName = applicationUserEditModel.Email;
            // user.NormalizedUserName = applicationUserEditModel.Email.ToUpper();
            // user.NormalizedEmail = applicationUserEditModel.Email.ToUpper();

            _context.Update(user);

            _context.SaveChanges();
            
            
        }

        public void DeleteApplicationUser(ApplicationUser appusr)
        {
            _context.ApplicationUser.Remove(appusr);
            _context.SaveChanges();
        }

        public bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(x => x.Id == id);
        }

        public string getUserFirstNameByEmail(string email)
        {
            string rez = _context.ApplicationUser.SingleOrDefault(m => m.Email == email).FirstName.Decrypt();
            return rez;
        }

        public string DetermineUserRole(string id)
        {
            string usrrole = (from appUsr in _context.ApplicationUser
                           join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                           join role in _context.Roles on usrRoles.RoleId equals role.Id
                           where appUsr.Id == id
                          select role.Name).Single();
            return usrrole.ToUpper();
        }
        
        public int GetAgeOfUser(string id)
        {
            ApplicationUser usr = GetUserById(id);

            DateTime today = DateTime.Today;

            int age = today.Year - usr.BirthDate.Year;
            // Go back to the year the person was born in case of a leap year
            if (usr.BirthDate > today.AddYears((-1) * age))
                age-=1;
            return age;
        }

        public string GetFullNameById(string id)
        {
            ApplicationUser usr = GetUserById(id);

            return usr.LastName.Decrypt() + " " + usr.FirstName.Decrypt();
        }

        public List<string> GetAllSpecializations()
        {
            List<string> Rez = (from appUsr in _context.ApplicationUser
                                join usrRoles in _context.UserRoles on appUsr.Id equals usrRoles.UserId
                                join role in _context.Roles on usrRoles.RoleId equals role.Id
                                where role.Name == "Medic"
                                select appUsr.Title.Decrypt().ToUpper().Replace("MEDIC ","")).Distinct().OrderBy(x=>x).ToList();
            
            
            return Rez;
        }

        public List<ApplicationUser> GetAllMedicsForCertainSpecialization(string specialization)
        {
            List<ApplicationUser> Rez = GetAllMedics().Where(x => x.Title.ToLower().Contains(specialization.ToLower())).ToList();

            foreach(ApplicationUser usr in Rez){
                usr.FirstName = usr.FirstName.Decrypt();
                usr.LastName = usr.LastName.Decrypt();
                usr.PhoneNumber = usr.PhoneNumber.Decrypt();
                if (usr.Title != null || usr.Title != "")
                {
                    usr.Title = usr.Title.Decrypt();
                }
                if (usr.CabinetAddress != null || usr.CabinetAddress != "")
                {
                    usr.CabinetAddress = usr.CabinetAddress.Decrypt();
                }
            }
            return Rez;
        }

        public List<ApplicationUser> GetMedicListByLocation(string location)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfThisUserHaveAProfilePicture(Guid id)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Users/" + id);
            if (Directory.Exists(searchedPath))
            {
                return true;
            }
            return false;
        }
        

        public string GetProfilePictureLink(string id)
        {

            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Users\\" + id;

            if (!Directory.Exists(path))
            {
                if (DetermineUserRole(id) == "MEDIC")
                {
                    return "/Users/Default/Default_Medic.png";
                }
                else
                {
                    return "/Users/Default/Default_User.png";
                }
            }
            else { 
                string fileName = GetNameOfProfilePictureById(new Guid(id));
                return "/Users/" + id + "/"+fileName;
            }
        }

        public void DeleteProfilePictureFilesForGivenId(Guid id)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Users/" + id);
            if (Directory.Exists(searchedPath))
            {
                Directory.Delete(searchedPath, true);
            }
        }

        public void DeleteCertainFile(Guid userId, string fileName)
        {
            var searchedPath = Path.Combine(_env.WebRootPath, "Users/" + userId + "/" + fileName);

            if (File.Exists(searchedPath))
            {
                File.Delete(searchedPath);
            }
        }


        public string GetNameOfProfilePictureById(Guid userId)
        {
            string fileList = "";
            string path = Directory.GetCurrentDirectory() + "\\wwwroot\\Users\\" + userId;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var files in Directory.GetFiles(path))
            {
                fileList = Path.GetFileName(files);
            }

            return fileList;
        }
        
        public int GetNumberOfPagesForAllUsers()
        {
            int rez = 0;
            int count = _context.ApplicationUser.Count();
            rez = count / 5;

            if (rez * 5 < count)
            {
                rez++;
            }

            return rez;
        }
        public IEnumerable<ApplicationUser> Get5UsersByIndex(int index)
        {
            List<ApplicationUser> rez = new List<ApplicationUser>() { };
            List<ApplicationUser> allUsers = GetAllUsers();
            int start = (index - 1) * 5;
            int finish = start + 5;

            for (int i = start; i < finish; i++)
            {
                if (i < allUsers.Count)
                {
                    rez.Add(allUsers[i]);
                }
            }

            return rez;
        }

        public string getUrlBase()
        {
            return _env.WebRootPath;
        }

        public int GetNumberOfPagesForDoctors()
        {
            int rez = 0;
            int count = GetAllMedics().Count();
            rez = count / 5;

            if (rez * 5 < count)
            {
                rez++;
            }

            return rez;
        }

        public IEnumerable<ApplicationUser> Get5DoctorsByIndex(int index)
        {
            List<ApplicationUser> rez = new List<ApplicationUser>() { };
            List<ApplicationUser> allMedics = GetAllMedics();
            int start = (index - 1) * 5;
            int finish = start + 5;

            for (int i = start; i < finish; i++)
            {
                if (i < allMedics.Count)
                {
                    rez.Add(allMedics[i]);
                }
            }

            return rez;
        }

        public int GetNumberOfPagesForPacients()
        {
            int rez = 0;
            int count = GetAllPacients().Count();
            rez = count / 5;

            if (rez * 5 < count)
            {
                rez++;
            }

            return rez;
        }

        public IEnumerable<ApplicationUser> Get5PacientsByIndex(int index)
        {
            List<ApplicationUser> rez = new List<ApplicationUser>() { };
            List<ApplicationUser> allPacients = GetAllPacients();
            int start = (index - 1) * 5;
            int finish = start + 5;

            for (int i = start; i < finish; i++)
            {
                if (i < allPacients.Count)
                {
                    rez.Add(allPacients[i]);
                }
            }

            return rez;
        }

        public bool Exists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }


        public async Task UploadProfilePicture(string id, IEnumerable<IFormFile> UploadedFile)
        {
            var extensions = new List<string>
            {
                ".png",
                ".jpg",
                ".jpeg",
                ".bpg",
                ".svg"
            };
            
            if (UploadedFile != null && UploadedFile.Count() == 1)
            {
                bool ok = false;

                foreach (var file in UploadedFile)
                {
                    string fileName = file.FileName;

                    int count = fileName.Count(x => x == '.');

                    if (count == 1)
                    {
                        foreach (string extention in extensions)
                        {
                            if (fileName.EndsWith(extention))
                            {
                                ok = true;
                            }
                        }
                    }

                }
                if (ok == true)
                {
                   
                    var path = Path.Combine(_env.WebRootPath, "Users/" + id);

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Sterg ce aveam inainte si rescriu
                    
                    Guid UId = new Guid(id);
                    string currentFileName = GetNameOfProfilePictureById(UId);
                    DeleteCertainFile(UId, currentFileName);

                    foreach (var file in UploadedFile)
                    {
                        if (file.Length > 0)
                        {
                            using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
            }
        }

        public void SetActive(string id)
        {
            ApplicationUser user = GetUserById(id);
            if (!GetOPUSers().Contains(user))
            {
                user.ActiveAccount = true;

                _context.Update(user);

                _context.SaveChanges();
            }
        }

        public void SetInactive(string id)
        {
            ApplicationUser user = GetUserById(id);

            if(!GetOPUSers().Contains(user)){
                user.ActiveAccount = false;

                _context.Update(user);

                _context.SaveChanges();
            }
        }
        public string GetActivity(string id)
        {
            ApplicationUser user = GetUserById(id);

            return user.ActiveAccount.ToString();
        }
    }
}
