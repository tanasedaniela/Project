using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediArch.Models;
using MediArch.Models.AccountViewModels;
using MediArch.Services;
using MediArch.Enums;
using MediArch.Data;
using Microsoft.EntityFrameworkCore;
using MediArch.Models.ApplicationUserViewModels;
using MediArch.Services.Interfaces;

namespace MediArch.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _databaseService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        private readonly IApplicationUserService _service;

        public AccountController(
            ApplicationDbContext databaseService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
           
            ILogger<AccountController> logger,
            IApplicationUserService service
           )
        {
            _databaseService = databaseService;
            _userManager = userManager;
            _signInManager = signInManager;
            
            _logger = logger;
            _service = service;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        
        public ActionResult SearchForAllUsers(string text)
        {
            return Json(_service.SearchUsers(text));
        }

        public ActionResult SearchMedics(string text)
        {
            return Json(_service.SearchMedics(text));
        }

        public ActionResult SearchPacients(string text)
        {
            return Json(_service.SearchPacients(text));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
               

                if (result.Succeeded)
                {
                    if (_service.GetActivity(_service.GetUserByUserName(model.Email).Id) == "False")
                    {
                        await _signInManager.SignOutAsync();
                        _logger.LogInformation("User logged out.");
                        return View("InactiveAccount");
                    }

                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
                }

               

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
      

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterMedic(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterMedic(RegisterMedicViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                int ok = 1; // 1 = It's ok
                foreach (var x in _databaseService.Users.ToList())
                {
                    if (x.Email.Equals(model.Email))
                    {   // Email already used
                        ok = 3;
                    }
                    else
                        if (model.BirthDate > DateTime.Now)
                        {   // Person must be already born
                            ok = 5;
                        }
                        else 
                            if(((int)model.BirthDate.Year > (int)((int)DateTime.Now.Year - 24)) && (model.BirthDate<=DateTime.Now))
                            {   // An < 24 years old person can't be an medic
                                ok = 6;
                            }
                            else
                                if ((int)model.BirthDate.Year < (int)((int)DateTime.Now.Year - 250)) // There is not person older then 250 years
                                {
                                    ok = 4;
                                }

                }

                if (ok == 1)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Title = model.Title,
                        CabinetAddress = model.CabinetAddress,
                        ActiveAccount = true,
                        CreatedDate = DateTime.Now
                    };

                    user.FirstName = user.FirstName.Encrypt();
                    user.LastName = user.LastName.Encrypt();
                    user.PhoneNumber = user.PhoneNumber.Encrypt();
                    user.Title = user.Title.Encrypt();
                    user.CabinetAddress = user.CabinetAddress.Encrypt();


                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {

                        var result2 = await _userManager.AddToRoleAsync(user, UserRoles.Medic.ToString());

                        if (result2.Succeeded)
                        {
                            _logger.LogInformation("User created a new account with password.");

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            _logger.LogInformation("User created a new account with password.");

                            return RedirectToLocal(returnUrl);
                        }
                        
                    }

                    AddErrors(result);
                }

                if (ok == 3)
                {   // Email
                    AddStringErrors("This mail was already used!");
                }
                if(ok == 4)
                {   // age > 250 years
                    AddStringErrors("There is no existing person with this age!");
                }
                if (ok == 5)
                {   // unborn yet
                    AddStringErrors("Create your account when u'll be born!(Inserted date is from the future)");
                }
                if (ok == 6)
                { // < 24 years old person can't be an medic
                    AddStringErrors("An person which is less then 24 can't be an doctor");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterPacient(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPacient(RegisterPacientViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            
            if (ModelState.IsValid)
            {

                int ok = 1;
                foreach (var x in _databaseService.Users.ToList())
                {
                    if (x.Email.Equals(model.Email))
                    {
                        ok = 3;
                    }
                    else
                        if (model.BirthDate > DateTime.Now)  // Person must be already born
                        {
                            ok = 5;
                        }
                        else
                            if ((int)model.BirthDate.Year < (int)((int)DateTime.Now.Year - 250)) // There is not person older then 250 years
                            {
                                ok = 4;
                            }

                }

                if (ok == 1)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        BirthDate = model.BirthDate,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        ActiveAccount = true,
                        CreatedDate = DateTime.Now
                    };

                    user.FirstName = user.FirstName.Encrypt();
                    user.LastName = user.LastName.Encrypt();
                    user.PhoneNumber = user.PhoneNumber.Encrypt();

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        var result2 = await _userManager.AddToRoleAsync(user, UserRoles.Pacient.ToString());

                        if (result2.Succeeded)
                        {
                            _logger.LogInformation("User created a new account with password.");

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            

                            await _signInManager.SignInAsync(user, isPersistent: false);
                            _logger.LogInformation("User created a new account with password.");

                            return RedirectToLocal(returnUrl);
                        }
                        
                    }
                    AddErrors(result);
                }

                if (ok == 3)
                {   // Email
                    AddStringErrors("This mail was already used!");
                }
                if (ok == 4)
                {   // age > 250 years
                    AddStringErrors("There is no existing person with this age!");
                }
                if (ok == 5)
                {   // unborn yet
                    AddStringErrors("Create your account when u'll be born!(Inserted date is from the future)");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

            

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private void AddStringErrors(string result)
        {
            ModelState.AddModelError(string.Empty, result);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        //

        // GET: ApplicationUsers
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Index()
        {
            //return View(_service.SearchUsers("alex"));
            return View(_service.GetAllUsers());
        }
        
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Users(int noPage)
        {
            //return View(_service.GetAllUsers());
            if (noPage < 1)
            {
                noPage = 1;
            }
            if (noPage > _service.GetNumberOfPagesForAllUsers())
            {
                noPage = _service.GetNumberOfPagesForAllUsers();
            }
            return View(_service.Get5UsersByIndex(noPage));
        }

        // GET: ApplicationUsers
        [Authorize(Roles = "Owner, Moderator, Medic")]
        public IActionResult GetPacientList()
        {
            return View(_service.GetAllPacients());
        }
        
        [Authorize(Roles = "Owner, Moderator, Medic")]
        public IActionResult Pacients(int noPage)
        {
            if (noPage < 1)
            {
                noPage = 1;
            }
            if (noPage > _service.GetNumberOfPagesForPacients())
            {
                noPage = _service.GetNumberOfPagesForPacients();
            }
            return View(_service.Get5PacientsByIndex(noPage));
        }

        // GET: ApplicationUsers
        [Authorize(Roles = "Owner, Moderator, Medic, Pacient")]
        public IActionResult GetMedicList()
        {
            return View(_service.GetAllMedics());
        }

        [Authorize(Roles = "Owner, Moderator, Medic, Pacient")]
        public IActionResult Doctors(int noPage)
        {
            if (noPage < 1)
            {
                noPage = 1;
            }
            if (noPage > _service.GetNumberOfPagesForDoctors())
            {
                noPage = _service.GetNumberOfPagesForDoctors();
            }
            return View(_service.Get5DoctorsByIndex(noPage));
        }

        // GET: ApplicationUsers
        [Authorize(Roles = "Owner, Moderator, Medic, Pacient")]
        public IActionResult GetMedicListForEachSpecialization()
        {
            return View();
        }

        // GET: ApplicationUsers/Details/5
        [Authorize(Roles = "Owner, Moderator, Medic, Pacient")]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var applicationUser = _service.GetUserById(id);
            if (applicationUser == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(applicationUser);
        }


        // GET: ApplicationUsers/Edit/5
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            ApplicationUser applicationUser = _service.GetUserById(id.ToString());
            if (applicationUser == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            ApplicationUserEditModel applicationUserEditModel = new ApplicationUserEditModel(
                applicationUser.Id,
                applicationUser.FirstName,
                applicationUser.LastName,
                applicationUser.BirthDate,
                applicationUser.Title,
                applicationUser.CabinetAddress,
                applicationUser.PhoneNumber
            );

            return View(applicationUserEditModel);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult Edit(string id, ApplicationUserEditModel applicationUserEditModel)
        {
            if (id != applicationUserEditModel.Id)
            {
                return RedirectToAction("Not_Found", "Home");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _service.EditApplicationUser(applicationUserEditModel);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUserEditModel.Id.ToString()))
                    {
                        return RedirectToAction("Not_Found", "Home");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Users));
            }
            return View(applicationUserEditModel);
        }

        
        public async Task<IActionResult> SetActive(string id)
        {
            _service.SetActive(id);
            ApplicationUser usr = _service.GetUserById(id);
            

         
            return RedirectToAction("Details", "Account", new { id = id });
        }

        
        public async Task<IActionResult> SetInactive(string id)
        {
            _service.SetInactive(id);

            ApplicationUser usr = _service.GetUserById(id);

        

            return RedirectToAction("Details", "Account", new { id = id });
        }


        private bool ApplicationUserExists(string id)
        {
            return ApplicationUserExists(id);
        }

        // GET: ApplicationUsers/Delete/5
        [Authorize(Roles = "Owner")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            var applicationUser = _service.GetUserById(id);
            if (applicationUser == null)
            {
                return RedirectToAction("Not_Found", "Home");
            }

            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Moderator")]
        public IActionResult DeleteConfirmed(string id)
        {
            var applicationUser = _service.GetUserById(id);
            _service.DeleteApplicationUser(applicationUser);
            return RedirectToAction(nameof(Users));
        }
        #endregion
    }
}
