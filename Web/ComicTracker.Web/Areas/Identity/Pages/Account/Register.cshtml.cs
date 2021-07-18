namespace ComicTracker.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
#pragma warning disable SA1210 // Using directives should be ordered alphabetically by namespace
    using Microsoft.AspNetCore.Authorization;
#pragma warning restore SA1210 // Using directives should be ordered alphabetically by namespace
    using ComicTracker.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
#pragma warning disable SA1309 // Field names should not begin with underscore
        private readonly SignInManager<ApplicationUser> _signInManager;
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning disable SA1309 // Field names should not begin with underscore
        private readonly UserManager<ApplicationUser> _userManager;
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning disable SA1309 // Field names should not begin with underscore
        private readonly ILogger<RegisterModel> _logger;
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning disable SA1309 // Field names should not begin with underscore
        private readonly IEmailSender _emailSender;
#pragma warning restore SA1309 // Field names should not begin with underscore

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
#pragma warning disable SA1101 // Prefix local calls with this
            _userManager = userManager;
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
            _signInManager = signInManager;
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
            _logger = logger;
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
            _emailSender = emailSender;
#pragma warning restore SA1101 // Prefix local calls with this
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Username")]
            public string Username { get; set; }

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
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
        public async Task OnGetAsync(string returnUrl = null)
#pragma warning restore SA1201 // Elements should appear in the correct order
        {
#pragma warning disable SA1101 // Prefix local calls with this
            ReturnUrl = returnUrl;
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning restore SA1101 // Prefix local calls with this
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
#pragma warning disable SA1101 // Prefix local calls with this
            returnUrl ??= Url.Content("~/");
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
            if (ModelState.IsValid)
#pragma warning restore SA1101 // Prefix local calls with this
            {
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
                var user = new ApplicationUser { UserName = Input.Username, Email = Input.Email };
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
                var result = await _userManager.CreateAsync(user, Input.Password);
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning restore SA1101 // Prefix local calls with this
                if (result.Succeeded)
                {
#pragma warning disable SA1101 // Prefix local calls with this
                    _logger.LogInformation("User created a new account with password.");
#pragma warning restore SA1101 // Prefix local calls with this

#pragma warning disable SA1101 // Prefix local calls with this
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
#pragma warning restore SA1101 // Prefix local calls with this
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
#pragma warning disable SA1101 // Prefix local calls with this
                    var callbackUrl = Url.Page(
#pragma warning restore SA1101 // Prefix local calls with this
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
#pragma warning disable SA1101 // Prefix local calls with this
                        protocol: Request.Scheme);
#pragma warning restore SA1101 // Prefix local calls with this

#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
#pragma warning restore SA1117 // Parameters should be on same line or separate lines

#pragma warning disable SA1101 // Prefix local calls with this
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
#pragma warning restore SA1101 // Prefix local calls with this
                    {
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning restore SA1101 // Prefix local calls with this
                    }
                    else
                    {
#pragma warning disable SA1101 // Prefix local calls with this
                        await _signInManager.SignInAsync(user, isPersistent: false);
#pragma warning restore SA1101 // Prefix local calls with this
#pragma warning disable SA1101 // Prefix local calls with this
                        return LocalRedirect(returnUrl);
#pragma warning restore SA1101 // Prefix local calls with this
                    }
#pragma warning disable SA1513 // Closing brace should be followed by blank line
                }
                foreach (var error in result.Errors)
#pragma warning restore SA1513 // Closing brace should be followed by blank line
                {
#pragma warning disable SA1101 // Prefix local calls with this
                    ModelState.AddModelError(string.Empty, error.Description);
#pragma warning restore SA1101 // Prefix local calls with this
                }
            }

            // If we got this far, something failed, redisplay form
#pragma warning disable SA1101 // Prefix local calls with this
            return Page();
#pragma warning restore SA1101 // Prefix local calls with this
        }
    }
}
