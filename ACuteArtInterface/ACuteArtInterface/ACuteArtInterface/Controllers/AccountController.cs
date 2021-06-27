using AutoMapper;
using EmailService;
using ACuteArtInterface.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ACuteArtInterface.Factory;
using Microsoft.Extensions.Logging;

namespace ACuteArtInterface.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Uri ToUri(HttpRequest request, bool withPath)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = hostComponents[0],
                //Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            if (withPath)
            {
                builder.Path = request.Path;
            }

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            return builder.Uri;
        }

        public AccountController(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<AccountController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Register methods
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            //TODO: commented out the registration so it needed to redirect to the home page
            //return View();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="userModel">FirstName, LastName, E-Mail and Password inputs</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = _mapper.Map<ApplicationUser>(userModel);

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Request -> Register -> Model is Succeeded");

                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Request -> Register -> Code : {error.Code}, Description: {error.Description}");
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View(userModel);
            }

            await _userManager.AddToRoleAsync(user, "StandartUser");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Login check
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Login with user model
        /// </summary>
        /// <param name="userModel">UserName and Password</param>
        /// <param name="returnUrl">if login successfull go to main page</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Request -> Login -> Model is not valid");
                return View(userModel);
            }

            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Request -> Login -> Succeded");
                return RedirectToLocal(returnUrl);
            }
            else
            {
                _logger.LogError($"Request -> Login -> Invalid UserName or Password");
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View();
            }
        }

        /// <summary>
        /// Logout and return home
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogError($"Request -> Logout");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Forgot password page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Create Forgot password link with token and send an email to the user
        /// outside of the login system
        /// </summary>
        /// <param name="forgotPasswordModel">E-Mail address</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);

            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            string content = generateForgotPasswordContent(callback, user);

            var sendGridMessage = new SendGridMessage(user.Email, "Reset password instructions", "", content);
            await _emailSender.SendGridEmailAsync(sendGridMessage);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        /// <summary>
        /// Create Forgot password link with token and send an email to the user
        /// Inside of the Account menu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordByEmail()
        {
            if (User == null || User.Identity == null || string.IsNullOrEmpty(User.Identity.Name))
            {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            string content = generateForgotPasswordContent(callback, user);

            var sendGridMessage = new SendGridMessage(user.Email, "Reset password instructions", "", content);
            await _emailSender.SendGridEmailAsync(sendGridMessage);

            //var message = new Message(new string[] { user.Email }, "Reset password instructions", text, null);
            //await _emailSender.SendEmailAsync(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callBackUrl"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private string generateForgotPasswordContent(string callBackUrl, ApplicationUser user)
        {
            Uri absoluteUri = ToUri(_httpContextAccessor.HttpContext.Request, false);

            return "<!DOCTYPE html>" +
                            "<html>" +
                            "<head>" +
                                "<meta charset='utf-8' />" +
                                "<title>Forgot Password</title>" +
                            "</head>" +
                            "<body>" +
                                "<p style='color: black; font-size:medium'>Hello " + user.FirstName + " " + user.LastName + ",</p>" +
                                "<p></p>" +
                                "<p style='color: black; font-size: medium'>You requested a link to change you password." +
                                "<p></p>" +
                                "<p style='color: black; font-size: medium'>You can do this through the link below:" +
                                "<p style='color: black; font-size: medium'><a href='" + callBackUrl + "' style='font-size:medium' target='_blank'>Password Reset Link</a></p>" +
                                "<p style='color: black; font-size: medium'></p>" +
                                "<p style='color: black; font-size: medium'>If you have trouble using the link above, you can also confirm by copying the link below into your address bar:</p>" +
                                "<p style='color: black; font-size: medium'>" + callBackUrl + "</p>" +
                                "<p style='color: black; font-size: medium'></p>" +
                                "<p style='color: black; font-size: medium'>Your password won't change until you access the link aboove and create a new one.</p>" +
                                "<p></p>" +
                                "<p style='color: black; font-size: medium'>If you didn't request this, please ignore this email.</p>" +
                                "<p></p>" +
                                "<p><img src='" + absoluteUri.ToString() + "img/logo.png' alt='Acute Art logo' title='Acute Art' style='width:120px;height:75px'></p>" +
                            "</body>" +
                            "</html>";
        }

        /// <summary>
        /// forgot password confirmation page
        /// </summary>
        /// <returns></returns>
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// when a user clicks the forgot password link
        /// </summary>
        /// <param name="token"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        /// <summary>
        /// resets the password
        /// </summary>
        /// <param name="resetPasswordModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);

            var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirmation));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View();
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        /// <summary>
        /// After resetting the password confirmation page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// GET: Account/Edit/5
        /// Edits the user first and last name
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            UserEditModel userEditModel = new UserEditModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ID = user.Id
            };

            return View(userEditModel);
        }

        /// <summary>
        /// POST: Account/Edit/5
        /// Edits the user first and last name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rowVersion"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userToUpdate = await _userManager.FindByIdAsync(id);

            if (await TryUpdateModelAsync<ApplicationUser>(
                userToUpdate,
                "",
                i => i.FirstName, i => i.LastName))
            {
                try
                {
                    await _userManager.UpdateAsync(userToUpdate);

                    var userOrj = await _userManager.FindByEmailAsync(User.Identity.Name);

                    UpdateClaims(userOrj);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError($"Account -> Edit -> exception: {ex.ToString()}");

                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View(userToUpdate);
        }

        /// <summary>
        /// After changing the user information
        /// updating the claims
        /// </summary>
        /// <param name="user"></param>
        public void UpdateClaims(ApplicationUser user)
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
                return;

            var existingClaimFirstName = identity.FindFirst("firstname");
            if (existingClaimFirstName != null)
            {
                identity.RemoveClaim(existingClaimFirstName);
            }

            var existingClaimLastName = identity.FindFirst("lastname");
            if (existingClaimLastName != null)
            {
                identity.RemoveClaim(existingClaimLastName);
            }

            identity.AddClaim(new Claim("firstname", user.FirstName));
            identity.AddClaim(new Claim("lastname", user.LastName));


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}