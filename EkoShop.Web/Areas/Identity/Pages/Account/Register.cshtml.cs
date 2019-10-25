using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EkoShop.Models;
using EkoShop.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace EkoShop.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        [Obsolete]
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment env)
            
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Name { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string StreetAddress { get; set; }

            public string PhoneNumber { get; set; }


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //this to get the role of the user in the register form
            string role = Request.Form["rdUserRole"].ToString();
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    Name = Input.Name,
                    City = Input.City,
                    State = Input.State,
                    StreetAddress = Input.StreetAddress,
                    PhoneNumber = Input.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    //creating all the roles
                    if(!await _roleManager.RoleExistsAsync(SD.ManagerUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.ManagerUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.StaffUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.StaffUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.BlogUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.BlogUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.CustomerUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.CustomerUser));
                    }

                    //assigning a role to the user
                    if(role == SD.ManagerUser)
                    {
                        await _userManager.AddToRoleAsync(user, SD.ManagerUser);
                    }
                    else
                    {
                        if (role == SD.StaffUser)
                        {
                            await _userManager.AddToRoleAsync(user, SD.StaffUser);
                        }
                        else
                        {
                            if(role == SD.BlogUser)
                            {
                                await _userManager.AddToRoleAsync(user, SD.BlogUser);

                            }
                            else
                            {
                                //Only customers will be able to sign in immediately if their account is confirmed
                                await _userManager.AddToRoleAsync(user, SD.CustomerUser);
                               
                                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                                {
                                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                                }
                                else
                                {
                                    await _signInManager.SignInAsync(user, isPersistent: false);
                                    return LocalRedirect(returnUrl);
                                }
                            }
                        }
                    }
                    _logger.LogInformation("User created a new account with password. ==>" + user);
                   

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    string Message = "Please confirm your account";

                    var subject = "Confirm Account Registration";

                    //get webroot path
                    var webRoot = _env.WebRootPath;

                    //Get TemplateFile located at wwwroot/templates/ConfirmAccount.html
                    var pathToFile = _env.WebRootPath
                           + Path.DirectorySeparatorChar.ToString()
                           + "templates"
                           + Path.DirectorySeparatorChar.ToString()
                           + "ConfirmAccount.html";

                    var builder = new BodyBuilder();
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        builder.HtmlBody = SourceReader.ReadToEnd();
                    }
                    //{0} : Subject
                    //{1}:  DateTime
                    //{2}:  Name
                    //{3}: Message
                    //{4}: callbackUrl

                    string messageBody = string.Format(builder.HtmlBody,
                        subject,
                        String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                        Input.Name,
                        Message,
                        callbackUrl
                        );

                    await _emailSender.SendEmailAsync(Input.Email, subject, messageBody);

                  
                    //redirecting the admin back to user index page
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    //return RedirectToAction("Index", "User", new { area = "Admin" });

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
